/*
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/.
 * Copyright (c) Swan & The Quaver Team <support@quavergame.com>.
*/

using System.Collections.Generic;
using Quaver.API.Maps;
using Quaver.Shared.Config;
using Quaver.Shared.Screens.Gameplay.Rulesets.Keys.HitObjects;

namespace Quaver.Shared.Screens.Gameplay.Rulesets.Keys.Playfield.Lines
{
    public class TimingLineManager
    {
        /// <summary>
        ///     Timing Line object pool.
        /// </summary>
        private Queue<TimingLine> Pool { get; set; }

        /// <summary>
        ///     Timing Line information. Generated by this class with qua object.
        /// </summary>
        private Queue<TimingLineInfo> Info { get; set; }

        /// <summary>
        ///     Reference to the HitObjectManager
        /// </summary>
        public HitObjectManagerKeys HitObjectManager { get; }

        /// <summary>
        ///     Reference to the current Ruleset
        /// </summary>
        public GameplayRulesetKeys Ruleset { get; }

        /// <summary>
        ///     Initial size for the object pool
        /// </summary>
        private int InitialPoolSize { get; } = 6;

        /// <summary>
        ///     Convert from BPM to measure length in milliseconds. (4 beats)
        ///     Equals to 4 * 60 * 1000
        /// </summary>
        public float BpmToMeasureLengthMs { get; } = 240000;

        /// <summary>
        ///     The Scroll Direction of every Timing Line
        /// </summary>
        public ScrollDirection ScrollDirection { get; }

        /// <summary>
        ///     Target position when TrackPosition = 0
        /// </summary>
        private float TrackOffset { get; }

        /// <summary>
        ///     Size of every Timing Line
        /// </summary>
        private float SizeX { get; }

        /// <summary>
        ///     Position of every Timing Line
        /// </summary>
        private float PositionX { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="map"></param>
        /// <param name="ruleset"></param>
        public TimingLineManager(GameplayRulesetKeys ruleset, ScrollDirection direction, float targetY, float size, float offset)
        {
            TrackOffset = targetY;
            SizeX = size;
            PositionX = offset;
            ScrollDirection = direction;
            Ruleset = ruleset;
            HitObjectManager = (HitObjectManagerKeys)ruleset.HitObjectManager;
            GenerateTimingLineInfo(ruleset.Map);
            InitializeObjectPool();
        }

        /// <summary>
        ///     Generate Timing Line Information for the map
        /// </summary>
        /// <param name="map"></param>
        private void GenerateTimingLineInfo(Qua map)
        {
            Info = new Queue<TimingLineInfo>();
            for (var i = 0; i < map.TimingPoints.Count; i++)
            {
                // Get target position and increment
                // Target position has tolerance of 1ms so timing points dont overlap by chance
                var target = i + 1 < map.TimingPoints.Count ? map.TimingPoints[i + 1].StartTime - 1 : map.Length;
                var increment = BpmToMeasureLengthMs / map.TimingPoints[i].Bpm;

                // Initialize timing lines between current timing point and target position
                for (var songPos = map.TimingPoints[i].StartTime; songPos < target; songPos += increment)
                {
                    if (songPos < target)
                        Info.Enqueue(new TimingLineInfo(songPos, HitObjectManager.GetPositionFromTime(songPos)));
                }
            }
        }

        /// <summary>
        ///     Initialize the Timing Line Object Pool
        /// </summary>
        private void InitializeObjectPool()
        {
            Pool = new Queue<TimingLine>();
            while (Info.Count > 0)
            {
                if (HitObjectManager.CurrentTrackPosition - Info.Peek().TrackOffset > HitObjectManager.CreateObjectPosition)
                    CreatePoolObject(Info.Dequeue());
                else
                    break;
            }
        }

        /// <summary>
        ///     Update every object in the Timing Line Object Pool and create new objects if necessary
        /// </summary>
        public void UpdateObjectPool()
        {
            // Update line positions
            if (Pool.Count > 0)
            {
                foreach (var line in Pool)
                    line.UpdateSpritePosition(HitObjectManager.CurrentTrackPosition);
            }

            // Recycle necessary pool objects
            while (Pool.Count > 0 && Pool.Peek().CurrentTrackPosition > HitObjectManager.RecycleObjectPosition)
            {
                var line = Pool.Dequeue();
                if (Info.Count > 0)
                {
                    line.Info = Info.Dequeue();
                    line.UpdateSpritePosition(HitObjectManager.CurrentTrackPosition);
                    Pool.Enqueue(line);
                }
            }

            // Create new pool objects if they are in range
            while (Info.Count > 0 && HitObjectManager.CurrentTrackPosition - Info.Peek().TrackOffset > HitObjectManager.CreateObjectPosition)
                CreatePoolObject(Info.Dequeue());
        }

        /// <summary>
        ///     Create and add new Timing Line Object to the Object Pool
        /// </summary>
        /// <param name="info"></param>
        private void CreatePoolObject(TimingLineInfo info)
        {
            var line = new TimingLine(Ruleset, info, ScrollDirection, TrackOffset, SizeX, PositionX);
            line.UpdateSpritePosition(HitObjectManager.CurrentTrackPosition);
            Pool.Enqueue(line);
        }
    }
}
