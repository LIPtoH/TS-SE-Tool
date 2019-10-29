/*
   Copyright 2016-2019 LIPtoH <liptoh.codebase@gmail.com>

   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

       http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.
*/
using System.Collections.Generic;

namespace TS_SE_Tool
{
    public class ProfileData
    {
        public string CompanyName { get; set; }
        public int CachedExperiencePoints { get; set; }
        public int CachedDistance { get; set; }
        //user_data
        //public uint SomeTime { get; set; } //0
        public double RoadsExplored { get; set; } //4
        public int DeliveriesFinished { get; set; } //5
        public int OwnedGaradesSmall { get; set; } //7
        public int OwnedGaradesLarge { get; set; } //8
        public int GameTimeSpent { get; set; } //9
        public int RealTimeSpent { get; set; } //10
        public List<string> OwnedTruckList = new List<string>(); // 12
        public int OwnedTrucks { get; set; } //6
        public int OwnedTrailers { get; set; } //16
        //End
        public string ProfileName { get; set; }
        public int CreationTime { get; set; }
        public int SaveTime { get; set; }

        public int[] getPlayerLvl()
        {
            int CurrentLVL = 0;
            int lvlthreshhold = 0;
            int[] Result;
            foreach (int lvlstep in Globals.PlayerLevelUps)
            {
                lvlthreshhold += lvlstep;

                if (CachedExperiencePoints < lvlthreshhold)                
                    return Result = new int[] { CurrentLVL, lvlthreshhold};
                                   
                else                
                    CurrentLVL++;                
            }
            
            int finalthreshhold = Globals.PlayerLevelUps[Globals.PlayerLevelUps.Length - 1];

            do
            {
                lvlthreshhold += finalthreshhold;

                if (CachedExperiencePoints < lvlthreshhold)
                    return Result = new int[] { CurrentLVL, lvlthreshhold };
                else
                    CurrentLVL++;
            } while (true);
        }
    }
}
