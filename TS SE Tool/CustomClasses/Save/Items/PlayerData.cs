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
    public class PlayerData
    {
        //Economy
        public uint ExperiencePoints { get; set; }
        public byte[] PlayerSkills { get; set; }
        //Bank
        public uint AccountMoney { get; set; }
        //Player
        public string HQcity { get; set; }
        public string UserCompanyAssignedTruck { get; set; }
        public string UserCompanyAssignedTruckPlacement { get; set; }
        public string UserCompanyAssignedTrailer { get; set; }
        public List<string> UserCompanyAssignedSlaveTrailerPlacements = new List<string>();
        public string CurrentJob { get; set; }
        public string UserDriver { get; set; }
        //Profile data
        public string CompanyName { get; set; }
        public string CompanyLogo { get; set; }
        public int CreationTime { get; set; }


        public PlayerData(string _HQcity, uint _Expirience, byte[] _PlayerSkills, uint _AccountMoney)
        {
            HQcity = _HQcity;
            ExperiencePoints = _Expirience;
            PlayerSkills = _PlayerSkills;
            AccountMoney = _AccountMoney;
        }

        public PlayerData(string _CompanyName, int _CreationTime)
        {
            CompanyName = _CompanyName;
            CreationTime = _CreationTime;
        }

        public void UpdatePlayerSkills(byte _ADR, byte _LongDistances, byte _HeavyCargo, byte _FragileCargo, byte _UrgentJobs, byte _MechanicalSkill)
        {
            PlayerSkills[0] = _ADR;
            PlayerSkills[1] = _LongDistances;
            PlayerSkills[2] = _HeavyCargo;
            PlayerSkills[3] = _FragileCargo;
            PlayerSkills[4] = _UrgentJobs;
            PlayerSkills[5] = _MechanicalSkill;
        }

        public int[] getPlayerLvl()
        {
            int CurrentLVL = 0;
            int lvlthreshhold = 0;
            int[] Result;
            foreach (int lvlstep in Globals.PlayerLevelUps)
            {
                lvlthreshhold += lvlstep;

                if (ExperiencePoints < lvlthreshhold)                
                    return Result = new int[] { CurrentLVL, lvlthreshhold};
                                   
                else                
                    CurrentLVL++;                
            }
            
            int finalthreshhold = Globals.PlayerLevelUps[Globals.PlayerLevelUps.Length - 1];

            do
            {
                lvlthreshhold += finalthreshhold;

                if (ExperiencePoints < lvlthreshhold)
                    return Result = new int[] { CurrentLVL, lvlthreshhold };
                else
                    CurrentLVL++;
            } while (true);
        }

        public uint getPlayerExp(int _plLvl)
        {
            uint experience = 0;

            if (_plLvl < 0)
                _plLvl = 0;

            if (_plLvl > 150)
                _plLvl = 150;

            for (int i = 0; i < _plLvl; i++)
            {
                if ( i < Globals.PlayerLevelUps.Length)
                    experience += (uint)Globals.PlayerLevelUps[i];
                else
                    experience += (uint)Globals.PlayerLevelUps[Globals.PlayerLevelUps.Length - 1];
            }

            if (_plLvl < Globals.PlayerLevelUps.Length)
                experience += (uint)Globals.PlayerLevelUps[_plLvl] - 1;
            else
                experience += (uint)Globals.PlayerLevelUps[Globals.PlayerLevelUps.Length - 1] - 1;

            ExperiencePoints = experience;

            return experience;
        }

    }
}
