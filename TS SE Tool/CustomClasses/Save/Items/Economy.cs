using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading.Tasks;

using TS_SE_Tool.Utilities;
using TS_SE_Tool.Save.DataFormat;

namespace TS_SE_Tool.Save.Items
{
    class Economy
    {
        internal string bank { get; set; } = "";
        internal string player { get; set; } = "";

        internal List<string> companies { get; set; } = new List<string>();
        internal List<string> garages { get; set; } = new List<string>();
        internal List<string> garage_ignore_list { get; set; } = new List<string>();

        internal string game_progress { get; set; } = "";
        internal string event_queue { get; set; } = "";
        internal string mail_ctrl { get; set; } = "";
        internal string oversize_offer_ctrl { get; set; } = "";

        internal int game_time { get; set; } = 0;

        internal SCS_Float game_time_secs { get; set; } = 0;

        internal int game_time_initial { get; set; } = 0;
        internal int achievements_added { get; set; } = 0;

        internal bool new_game { get; set; } = false;

        internal int total_distance { get; set; } = 0;
        internal uint experience_points { get; set; } = 0;

        internal byte adr { get; set; } = 0;
        internal byte long_dist { get; set; } = 0;
        internal byte heavy { get; set; } = 0;
        internal byte fragile { get; set; } = 0;
        internal byte urgent { get; set; } = 0;
        internal byte mechanical { get; set; } = 0;

        internal byte[] playerSkills { get; set; } = new byte[6] { 0, 0, 0, 0, 0, 0 };

        internal List<SCS_Color> user_colors = new List<SCS_Color>();

        internal string delivery_log { get; set; } = "";
        internal string ferry_log { get; set; } = "";

        internal int stored_camera_mode { get; set; } = 0;
        internal int stored_actor_state { get; set; } = 0;
        internal int stored_high_beam_style { get; set; } = 0;

        internal Vector_2f stored_actor_windows_state { get; set; } = new Vector_2f();

        internal int stored_actor_wiper_mode { get; set; } = 0;
        internal int stored_actor_retarder { get; set; } = 0;
        internal int stored_display_mode { get; set; } = 0;
        internal int stored_dashboard_map_mode { get; set; } = 0;
        internal int stored_world_map_zoom { get; set; } = 0;
        internal int stored_online_job_id { get; set; } = 0;

        internal List<string> stored_online_gps_behind { get; set; } = new List<string>();
        internal List<string> stored_online_gps_ahead { get; set; } = new List<string>();
        internal List<string> stored_online_gps_behind_waypoints { get; set; } = new List<string>();
        internal List<string> stored_online_gps_ahead_waypoints { get; set; } = new List<string>();
        internal List<string> stored_online_gps_avoid_waypoints { get; set; } = new List<string>();

        internal string stored_special_job { get; set; } = "";

        internal string police_ctrl { get; set; } = "";

        internal int stored_map_state { get; set; } = 0;
        internal int stored_gas_pump_money { get; set; } = 0;

        internal SCS_Float stored_weather_change_timer { get; set; } = 0;

        internal int stored_current_weather { get; set; } = 0;

        internal SCS_Float stored_rain_wetness { get; set; } = 0;

        internal int time_zone { get; set; } = 0;

        internal string time_zone_name { get; set; } = "";

        internal Vector_3i last_ferry_position { get; set; } = new Vector_3i();

        internal bool stored_show_weigh { get; set; } = false;
        internal bool stored_need_to_weigh { get; set; } = false;

        internal Vector_3i stored_nav_start_pos { get; set; } = new Vector_3i();
        internal Vector_3i stored_nav_end_pos { get; set; } = new Vector_3i();

        internal List<string> stored_gps_behind { get; set; } = new List<string>();
        internal List<string> stored_gps_ahead { get; set; } = new List<string>();
        internal List<string> stored_gps_behind_waypoints { get; set; } = new List<string>();
        internal List<string> stored_gps_ahead_waypoints { get; set; } = new List<string>();
        internal List<string> stored_gps_avoid_waypoints { get; set; } = new List<string>();

        internal Vector_3i stored_start_tollgate_pos { get; set; } = new Vector_3i();

        internal int stored_tutorial_state { get; set; } = 0;

        internal List<string> stored_map_actions { get; set; } = new List<string>();

        internal int clean_distance_counter { get; set; } = 0;
        internal int clean_distance_max { get; set; } = 0;
        internal int no_cargo_damage_distance_counter { get; set; } = 0;
        internal int no_cargo_damage_distance_max { get; set; } = 0;
        internal int no_violation_distance_counter { get; set; } = 0;
        internal int no_violation_distance_max { get; set; } = 0;
        internal int total_real_time { get; set; } = 0;

        internal SCS_Float real_time_seconds { get; set; } = 0;

        internal List<string> visited_cities { get; set; } = new List<string>();
        internal List<int> visited_cities_count { get; set; } = new List<int>();

        internal string last_visited_city { get; set; } = "";

        internal List<UInt64> discovered_cutscene_items { get; set; } = new List<UInt64>();
        internal List<int> discovered_cutscene_items_states { get; set; } = new List<int>();

        internal List<string> unlocked_dealers { get; set; } = new List<string>();
        internal List<string> unlocked_recruitments { get; set; } = new List<string>();

        internal int total_screeshot_count { get; set; } = 0;
        internal int undamaged_cargo_row { get; set; } = 0;
        internal int service_visit_count { get; set; } = 0;

        internal Vector_3f last_service_pos { get; set; } = new Vector_3f();

        internal int gas_station_visit_count { get; set; } = 0;

        internal Vector_3f last_gas_station_pos { get; set; } = new Vector_3f();

        internal int emergency_call_count { get; set; } = 0;
        internal int ai_crash_count { get; set; } = 0;
        internal int truck_color_change_count { get; set; } = 0;
        internal int red_light_fine_count { get; set; } = 0;
        internal int cancelled_job_count { get; set; } = 0;
        internal int total_fuel_litres { get; set; } = 0;
        internal int total_fuel_price { get; set; } = 0;

        internal List<string> transported_cargo_types { get; set; } = new List<string>();

        internal int achieved_feats { get; set; } = 0;
        internal int discovered_roads { get; set; } = 0;

        internal List<UInt64> discovered_items { get; set; } = new List<UInt64>();

        internal List<string> drivers_offer { get; set; } = new List<string>();

        internal string freelance_truck_offer { get; set; } = "";

        internal int trucks_bought_online { get; set; } = 0;
        internal int special_cargo_timer { get; set; } = 0;

        internal List<string> screen_access_list { get; set; } = new List<string>();
        internal List<string> driver_pool { get; set; } = new List<string>();

        internal string registry { get; set; } = "";

        internal bool company_jobs_invitation_sent { get; set; } = false;

        internal UInt64 company_check_hash { get; set; } = 0;

        internal List<int> relations { get; set; } = new List<int>();

        internal List<string> bus_stops { get; set; } = new List<string>();

        internal string bus_job_log { get; set; } = "";

        internal int bus_experience_points { get; set; } = 0;
        internal int bus_total_distance { get; set; } = 0;
        internal int bus_finished_job_count { get; set; } = 0;
        internal int bus_cancelled_job_count { get; set; } = 0;
        internal int bus_total_passengers { get; set; } = 0;
        internal int bus_total_stops { get; set; } = 0;
        internal int bus_game_time { get; set; } = 0;
        internal int bus_playing_time { get; set; } = 0;

        internal Economy()
        { }

        internal Economy(string[] _input)
        {
            string tagLine = "", dataLine = "";

            foreach (string currentLine in _input)
            {
                if (currentLine.Contains(':'))
                {
                    string[] splittedLine = currentLine.Split(new char[] { ':' }, 2);

                    tagLine = splittedLine[0].Trim();
                    dataLine = splittedLine[1].Trim();
                }
                else
                {
                    tagLine = currentLine.Trim();
                    dataLine = "";
                }

                switch (tagLine)
                {
                    case "":
                        {
                            break;
                        }

                    case "bank":
                        {
                            bank = dataLine;
                            break;
                        }

                    case "player":
                        {
                            player = dataLine;
                            break;
                        }

                    case "companies":
                        {
                            companies.Capacity = int.Parse(dataLine);
                            break;
                        }

                    case var s when s.StartsWith("companies["):
                        {
                            companies.Add(dataLine);
                            break;
                        }

                    case "garages":
                        {
                            garages.Capacity = int.Parse(dataLine);
                            break;
                        }

                    case var s when s.StartsWith("garages["):
                        {
                            garages.Add(dataLine);
                            break;
                        }

                    case "garage_ignore_list":
                        {
                            garage_ignore_list.Capacity = int.Parse(dataLine);
                            break;
                        }

                    case var s when s.StartsWith("garage_ignore_list["):
                        {
                            garage_ignore_list.Add(dataLine);
                            break;
                        }

                    case "game_progress":
                        {
                            game_progress = dataLine;
                            break;
                        }

                    case "event_queue":
                        {
                            event_queue = dataLine;
                            break;
                        }

                    case "mail_ctrl":
                        {
                            mail_ctrl = dataLine;
                            break;
                        }

                    case "oversize_offer_ctrl":
                        {
                            oversize_offer_ctrl = dataLine;
                            break;
                        }

                    case "game_time":
                        {
                            game_time = int.Parse(dataLine);
                            break;
                        }

                    case "game_time_secs":
                        {
                            game_time_secs = dataLine;
                            break;
                        }

                    case "game_time_initial":
                        {
                            game_time_initial = int.Parse(dataLine);
                            break;
                        }

                    case "achievements_added":
                        {
                            achievements_added = int.Parse(dataLine);
                            break;
                        }

                    case "new_game":
                        {
                            new_game = bool.Parse(dataLine);
                            break;
                        }

                    case "total_distance":
                        {
                            total_distance = int.Parse(dataLine);
                            break;
                        }

                    case "experience_points":
                        {
                            experience_points = uint.Parse(dataLine);
                            break;
                        }

                    case "adr":
                        {
                            adr = byte.Parse(dataLine);
                            break;
                        }

                    case "long_dist":
                        {
                            long_dist = byte.Parse(dataLine);
                            break;
                        }

                    case "heavy":
                        {
                            heavy = byte.Parse(dataLine);
                            break;
                        }

                    case "fragile":
                        {
                            fragile = byte.Parse(dataLine);
                            break;
                        }

                    case "urgent":
                        {
                            urgent = byte.Parse(dataLine);
                            break;
                        }

                    case "mechanical":
                        {
                            mechanical = byte.Parse(dataLine);
                            break;
                        }

                    case "user_colors":
                        {
                            user_colors.Capacity = int.Parse(dataLine);
                            break;
                        }

                    case var s when s.StartsWith("user_colors["):
                        {
                            user_colors.Add(new SCS_Color(dataLine));
                            break;
                        }

                    case "delivery_log":
                        {
                            delivery_log = dataLine;
                            break;
                        }

                    case "ferry_log":
                        {
                            ferry_log = dataLine;
                            break;
                        }

                    case "stored_camera_mode":
                        {
                            stored_camera_mode = int.Parse(dataLine);
                            break;
                        }

                    case "stored_actor_state":
                        {
                            stored_actor_state = int.Parse(dataLine);
                            break;
                        }

                    case "stored_high_beam_style":
                        {
                            stored_high_beam_style = int.Parse(dataLine);
                            break;
                        }

                    case "stored_actor_windows_state":
                        {
                            stored_actor_windows_state = new Vector_2f(dataLine);
                            break;
                        }

                    case "stored_actor_wiper_mode":
                        {
                            stored_actor_wiper_mode = int.Parse(dataLine);
                            break;
                        }

                    case "stored_actor_retarder":
                        {
                            stored_actor_retarder = int.Parse(dataLine);
                            break;
                        }

                    case "stored_display_mode":
                        {
                            stored_display_mode = int.Parse(dataLine);
                            break;
                        }

                    case "stored_dashboard_map_mode":
                        {
                            stored_dashboard_map_mode = int.Parse(dataLine);
                            break;
                        }

                    case "stored_world_map_zoom":
                        {
                            stored_world_map_zoom = int.Parse(dataLine);
                            break;
                        }

                    case "stored_online_job_id":
                        {
                            stored_online_job_id = int.Parse(dataLine);
                            break;
                        }

                    case "stored_online_gps_behind":
                        {
                            stored_online_gps_behind.Capacity = int.Parse(dataLine);
                            break;
                        }

                    case var s when s.StartsWith("stored_online_gps_behind["):
                        {
                            stored_online_gps_behind.Add(dataLine);
                            break;
                        }

                    case "stored_online_gps_ahead":
                        {
                            stored_online_gps_ahead.Capacity = int.Parse(dataLine);
                            break;
                        }

                    case var s when s.StartsWith("stored_online_gps_ahead["):
                        {
                            stored_online_gps_ahead.Add(dataLine);
                            break;
                        }

                    case "stored_online_gps_behind_waypoints":
                        {
                            stored_online_gps_behind_waypoints.Capacity = int.Parse(dataLine);
                            break;
                        }

                    case var s when s.StartsWith("stored_online_gps_behind_waypoints["):
                        {
                            stored_online_gps_behind_waypoints.Add(dataLine);
                            break;
                        }

                    case "stored_online_gps_ahead_waypoints":
                        {
                            stored_online_gps_ahead_waypoints.Capacity = int.Parse(dataLine);
                            break;
                        }

                    case var s when s.StartsWith("stored_online_gps_ahead_waypoints["):
                        {
                            stored_online_gps_ahead_waypoints.Add(dataLine);
                            break;
                        }

                    case "stored_online_gps_avoid_waypoints":
                        {
                            stored_online_gps_avoid_waypoints.Capacity = int.Parse(dataLine);
                            break;
                        }

                    case var s when s.StartsWith("stored_online_gps_avoid_waypoints["):
                        {
                            stored_online_gps_avoid_waypoints.Add(dataLine);
                            break;
                        }

                    case "stored_special_job":
                        {
                            stored_special_job = dataLine;
                            break;
                        }

                    case "police_ctrl":
                        {
                            police_ctrl = dataLine;
                            break;
                        }

                    case "stored_map_state":
                        {
                            stored_map_state = int.Parse(dataLine);
                            break;
                        }

                    case "stored_gas_pump_money":
                        {
                            stored_gas_pump_money = int.Parse(dataLine);
                            break;
                        }

                    case "stored_weather_change_timer":
                        {
                            stored_weather_change_timer = dataLine;
                            break;
                        }

                    case "stored_current_weather":
                        {
                            stored_current_weather = int.Parse(dataLine);
                            break;
                        }

                    case "stored_rain_wetness":
                        {
                            stored_rain_wetness = dataLine;
                            break;
                        }

                    case "time_zone":
                        {
                            time_zone = int.Parse(dataLine);
                            break;
                        }

                    case "time_zone_name":
                        {
                            time_zone_name = dataLine;
                            break;
                        }

                    case "last_ferry_position":
                        {
                            last_ferry_position = new Vector_3i(dataLine);
                            break;
                        }

                    case "stored_show_weigh":
                        {
                            stored_show_weigh = bool.Parse(dataLine);
                            break;
                        }

                    case "stored_need_to_weigh":
                        {
                            stored_need_to_weigh = bool.Parse(dataLine);
                            break;
                        }

                    case "stored_nav_start_pos":
                        {
                            stored_nav_start_pos = new Vector_3i(dataLine);
                            break;
                        }

                    case "stored_nav_end_pos":
                        {
                            stored_nav_end_pos = new Vector_3i(dataLine);
                            break;
                        }

                    case "stored_gps_behind":
                        {
                            stored_gps_behind.Capacity = int.Parse(dataLine);
                            break;
                        }

                    case var s when s.StartsWith("stored_gps_behind["):
                        {
                            stored_gps_behind.Add(dataLine);
                            break;
                        }

                    case "stored_gps_ahead":
                        {
                            stored_gps_ahead.Capacity = int.Parse(dataLine);
                            break;
                        }

                    case var s when s.StartsWith("stored_gps_ahead["):
                        {
                            stored_gps_ahead.Add(dataLine);
                            break;
                        }

                    case "stored_gps_behind_waypoints":
                        {
                            stored_gps_behind_waypoints.Capacity = int.Parse(dataLine);
                            break;
                        }

                    case var s when s.StartsWith("stored_gps_behind_waypoints["):
                        {
                            stored_gps_behind_waypoints.Add(dataLine);
                            break;
                        }

                    case "stored_gps_ahead_waypoints":
                        {
                            stored_gps_ahead_waypoints.Capacity = int.Parse(dataLine);
                            break;
                        }

                    case var s when s.StartsWith("stored_gps_ahead_waypoints["):
                        {
                            stored_gps_ahead_waypoints.Add(dataLine);
                            break;
                        }

                    case "stored_gps_avoid_waypoints":
                        {
                            stored_gps_avoid_waypoints.Capacity = int.Parse(dataLine);
                            break;
                        }

                    case var s when s.StartsWith("stored_gps_avoid_waypoints["):
                        {
                            stored_gps_avoid_waypoints.Add(dataLine);
                            break;
                        }

                    case "stored_tutorial_state":
                        {
                            stored_tutorial_state = int.Parse(dataLine);
                            break;
                        }

                    case "stored_map_actions":
                        {
                            stored_map_actions.Capacity = int.Parse(dataLine);
                            break;
                        }

                    case var s when s.StartsWith("stored_map_actions["):
                        {
                            stored_map_actions.Add(dataLine);
                            break;
                        }

                    case "clean_distance_counter":
                        {
                            clean_distance_counter = int.Parse(dataLine);
                            break;
                        }

                    case "clean_distance_max":
                        {
                            clean_distance_max = int.Parse(dataLine);
                            break;
                        }

                    case "no_cargo_damage_distance_counter":
                        {
                            no_cargo_damage_distance_counter = int.Parse(dataLine);
                            break;
                        }

                    case "no_cargo_damage_distance_max":
                        {
                            no_cargo_damage_distance_max = int.Parse(dataLine);
                            break;
                        }

                    case "no_violation_distance_counter":
                        {
                            no_violation_distance_counter = int.Parse(dataLine);
                            break;
                        }

                    case "no_violation_distance_max":
                        {
                            no_violation_distance_max = int.Parse(dataLine);
                            break;
                        }

                    case "total_real_time":
                        {
                            total_real_time = int.Parse(dataLine);
                            break;
                        }

                    case "real_time_seconds":
                        {
                            real_time_seconds = dataLine;
                            break;
                        }

                    case "visited_cities":
                        {
                            visited_cities.Capacity = int.Parse(dataLine);
                            break;
                        }

                    case var s when s.StartsWith("visited_cities["):
                        {
                            visited_cities.Add(dataLine);
                            break;
                        }

                    case "visited_cities_count":
                        {
                            visited_cities_count.Capacity = int.Parse(dataLine);
                            break;
                        }

                    case var s when s.StartsWith("visited_cities_count["):
                        {
                            visited_cities_count.Add(int.Parse(dataLine));
                            break;
                        }

                    case "last_visited_city":
                        {
                            last_visited_city = dataLine;
                            break;
                        }

                    case "discovered_cutscene_items":
                        {
                            discovered_cutscene_items.Capacity = int.Parse(dataLine);
                            break;
                        }

                    case var s when s.StartsWith("discovered_cutscene_items["):
                        {
                            discovered_cutscene_items.Add(UInt64.Parse(dataLine));
                            break;
                        }

                    case "discovered_cutscene_items_states":
                        {
                            discovered_cutscene_items_states.Capacity = int.Parse(dataLine);
                            break;
                        }

                    case var s when s.StartsWith("discovered_cutscene_items_states["):
                        {
                            discovered_cutscene_items_states.Add(int.Parse(dataLine));
                            break;
                        }

                    case "unlocked_dealers":
                        {
                            unlocked_dealers.Capacity = int.Parse(dataLine);
                            break;
                        }

                    case var s when s.StartsWith("unlocked_dealers["):
                        {
                            unlocked_dealers.Add(dataLine);
                            break;
                        }

                    case "unlocked_recruitments":
                        {
                            unlocked_recruitments.Capacity = int.Parse(dataLine);
                            break;
                        }

                    case var s when s.StartsWith("unlocked_recruitments["):
                        {
                            unlocked_recruitments.Add(dataLine);
                            break;
                        }

                    case "total_screeshot_count":
                        {
                            total_screeshot_count = int.Parse(dataLine);
                            break;
                        }

                    case "undamaged_cargo_row":
                        {
                            undamaged_cargo_row = int.Parse(dataLine);
                            break;
                        }

                    case "service_visit_count":
                        {
                            service_visit_count = int.Parse(dataLine);
                            break;
                        }

                    case "last_service_pos":
                        {
                            last_service_pos = new Vector_3f(dataLine);
                            break;
                        }

                    case "gas_station_visit_count":
                        {
                            gas_station_visit_count = int.Parse(dataLine);
                            break;
                        }

                    case "last_gas_station_pos":
                        {
                            last_gas_station_pos = new Vector_3f(dataLine);
                            break;
                        }

                    case "emergency_call_count":
                        {
                            emergency_call_count = int.Parse(dataLine);
                            break;
                        }

                    case "ai_crash_count":
                        {
                            ai_crash_count = int.Parse(dataLine);
                            break;
                        }

                    case "truck_color_change_count":
                        {
                            truck_color_change_count = int.Parse(dataLine);
                            break;
                        }

                    case "red_light_fine_count":
                        {
                            red_light_fine_count = int.Parse(dataLine);
                            break;
                        }

                    case "cancelled_job_count":
                        {
                            cancelled_job_count = int.Parse(dataLine);
                            break;
                        }

                    case "total_fuel_litres":
                        {
                            total_fuel_litres = int.Parse(dataLine);
                            break;
                        }

                    case "total_fuel_price":
                        {
                            total_fuel_price = int.Parse(dataLine);
                            break;
                        }

                    case "transported_cargo_types":
                        {
                            transported_cargo_types.Capacity = int.Parse(dataLine);
                            break;
                        }

                    case var s when s.StartsWith("transported_cargo_types["):
                        {
                            transported_cargo_types.Add(dataLine);
                            break;
                        }

                    case "achieved_feats":
                        {
                            achieved_feats = int.Parse(dataLine);
                            break;
                        }

                    case "discovered_roads":
                        {
                            discovered_roads = int.Parse(dataLine);
                            break;
                        }

                    case "discovered_items":
                        {
                            discovered_items.Capacity = int.Parse(dataLine);
                            break;
                        }

                    case var s when s.StartsWith("discovered_items["):
                        {
                            discovered_items.Add(UInt64.Parse(dataLine));
                            break;
                        }

                    case "drivers_offer":
                        {
                            drivers_offer.Capacity = int.Parse(dataLine);
                            break;
                        }

                    case var s when s.StartsWith("drivers_offer["):
                        {
                            drivers_offer.Add(dataLine);
                            break;
                        }

                    case "freelance_truck_offer":
                        {
                            freelance_truck_offer = dataLine;
                            break;
                        }

                    case "trucks_bought_online":
                        {
                            trucks_bought_online = int.Parse(dataLine);
                            break;
                        }

                    case "special_cargo_timer":
                        {
                            special_cargo_timer = int.Parse(dataLine);
                            break;
                        }

                    case "screen_access_list":
                        {
                            screen_access_list.Capacity = int.Parse(dataLine);
                            break;
                        }

                    case var s when s.StartsWith("screen_access_list["):
                        {
                            screen_access_list.Add(dataLine);
                            break;
                        }

                    case "driver_pool":
                        {
                            driver_pool.Capacity = int.Parse(dataLine);
                            break;
                        }

                    case var s when s.StartsWith("driver_pool["):
                        {
                            driver_pool.Add(dataLine);
                            break;
                        }

                    case "registry":
                        {
                            registry = dataLine;
                            break;
                        }

                    case "company_jobs_invitation_sent":
                        {
                            company_jobs_invitation_sent = bool.Parse(dataLine);
                            break;
                        }

                    case "company_check_hash":
                        {
                            company_check_hash = UInt64.Parse(dataLine);
                            break;
                        }

                    case "relations":
                        {
                            relations.Capacity = int.Parse(dataLine);
                            break;
                        }

                    case var s when s.StartsWith("relations["):
                        {
                            relations.Add(int.Parse(dataLine));
                            break;
                        }

                    case "bus_stops":
                        {
                            bus_stops.Capacity = int.Parse(dataLine);
                            break;
                        }

                    case var s when s.StartsWith("bus_stops["):
                        {
                            bus_stops.Add(dataLine);
                            break;
                        }

                    case "bus_job_log":
                        {
                            bus_job_log = dataLine;
                            break;
                        }

                    case "bus_experience_points":
                        {
                            bus_experience_points = int.Parse(dataLine);
                            break;
                        }

                    case "bus_total_distance":
                        {
                            bus_total_distance = int.Parse(dataLine);
                            break;
                        }

                    case "bus_finished_job_count":
                        {
                            bus_finished_job_count = int.Parse(dataLine);
                            break;
                        }

                    case "bus_cancelled_job_count":
                        {
                            bus_cancelled_job_count = int.Parse(dataLine);
                            break;
                        }

                    case "bus_total_passengers":
                        {
                            bus_total_passengers = int.Parse(dataLine);
                            break;
                        }

                    case "bus_total_stops":
                        {
                            bus_total_stops = int.Parse(dataLine);
                            break;
                        }

                    case "bus_game_time":
                        {
                            bus_game_time = int.Parse(dataLine);
                            break;
                        }

                    case "bus_playing_time":
                        {
                            bus_playing_time = int.Parse(dataLine);
                            break;
                        }
                }

                //Populate helping variables
                setPlayerSkillsArray();
            }
        }

        internal string PrintOut(uint _version, string _nameless)
        {
            //get data from helping variables
            getPlayerSkillsFromArray();

            //
            string returnString = "";

            StringBuilder returnSB = new StringBuilder();

            returnSB.AppendLine("economy : " + _nameless + " {");

            returnSB.AppendLine(" bank: " + bank);
            returnSB.AppendLine(" player: " + player);

            returnSB.AppendLine(" companies: " + companies.Count);
            for (int i = 0; i < companies.Count; i++)
                returnSB.AppendLine(" companies[" + i + "]: " + companies[i]);

            returnSB.AppendLine(" garages: " + garages.Count);
            for (int i = 0; i < garages.Count; i++)
                returnSB.AppendLine(" garages[" + i + "]: " + garages[i]);

            returnSB.AppendLine(" garage_ignore_list: " + garage_ignore_list.Count);
            for (int i = 0; i < garage_ignore_list.Count; i++)
                returnSB.AppendLine(" garage_ignore_list[" + i + "]: " + garage_ignore_list[i]);

            returnSB.AppendLine(" game_progress: " + game_progress);

            returnSB.AppendLine(" event_queue: " + event_queue);

            returnSB.AppendLine(" mail_ctrl: " + mail_ctrl);
            returnSB.AppendLine(" oversize_offer_ctrl: " + oversize_offer_ctrl);

            returnSB.AppendLine(" game_time: " + game_time.ToString());
            returnSB.AppendLine(" game_time_secs: " + game_time_secs.ToString());

            returnSB.AppendLine(" game_time_initial: " + game_time_initial.ToString());

            returnSB.AppendLine(" achievements_added: " + achievements_added.ToString());

            returnSB.AppendLine(" new_game: " + new_game.ToString().ToLower());

            returnSB.AppendLine(" total_distance: " + total_distance.ToString());

            returnSB.AppendLine(" experience_points: " + experience_points.ToString());

            returnSB.AppendLine(" adr: " + adr.ToString());
            returnSB.AppendLine(" long_dist: " + long_dist.ToString());
            returnSB.AppendLine(" heavy: " + heavy.ToString());
            returnSB.AppendLine(" fragile: " + fragile.ToString());
            returnSB.AppendLine(" urgent: " + urgent.ToString());
            returnSB.AppendLine(" mechanical: " + mechanical.ToString());

            returnSB.AppendLine(" user_colors: " + user_colors.Count);
            for (int i = 0; i < user_colors.Count; i++)
                returnSB.AppendLine(" user_colors[" + i + "]: " + user_colors[i].ToString());

            returnSB.AppendLine(" delivery_log: " + delivery_log);
            returnSB.AppendLine(" ferry_log: " + ferry_log);

            returnSB.AppendLine(" stored_camera_mode: " + stored_camera_mode.ToString());
            returnSB.AppendLine(" stored_actor_state: " + stored_actor_state.ToString());
            returnSB.AppendLine(" stored_high_beam_style: " + stored_high_beam_style.ToString());
            returnSB.AppendLine(" stored_actor_windows_state: " + stored_actor_windows_state.ToString());
            returnSB.AppendLine(" stored_actor_wiper_mode: " + stored_actor_wiper_mode.ToString());
            returnSB.AppendLine(" stored_actor_retarder: " + stored_actor_retarder.ToString());

            returnSB.AppendLine(" stored_display_mode: " + stored_display_mode.ToString());
            returnSB.AppendLine(" stored_dashboard_map_mode: " + stored_dashboard_map_mode.ToString());
            returnSB.AppendLine(" stored_world_map_zoom: " + stored_world_map_zoom.ToString());

            returnSB.AppendLine(" stored_online_job_id: " + stored_online_job_id.ToString());

            returnSB.AppendLine(" stored_online_gps_behind: " + stored_online_gps_behind.Count);
            for (int i = 0; i < stored_online_gps_behind.Count; i++)
                returnSB.AppendLine(" stored_online_gps_behind[" + i + "]: " + stored_online_gps_behind[i].ToString());

            returnSB.AppendLine(" stored_online_gps_ahead: " + stored_online_gps_ahead.Count);
            for (int i = 0; i < stored_online_gps_ahead.Count; i++)
                returnSB.AppendLine(" stored_online_gps_ahead[" + i + "]: " + stored_online_gps_ahead[i].ToString());

            returnSB.AppendLine(" stored_online_gps_behind_waypoints: " + stored_online_gps_behind_waypoints.Count);
            for (int i = 0; i < stored_online_gps_behind_waypoints.Count; i++)
                returnSB.AppendLine(" stored_online_gps_behind_waypoints[" + i + "]: " + stored_online_gps_behind_waypoints[i].ToString());

            returnSB.AppendLine(" stored_online_gps_ahead_waypoints: " + stored_online_gps_ahead_waypoints.Count);
            for (int i = 0; i < stored_online_gps_ahead_waypoints.Count; i++)
                returnSB.AppendLine(" stored_online_gps_ahead_waypoints[" + i + "]: " + stored_online_gps_ahead_waypoints[i].ToString());

            returnSB.AppendLine(" stored_online_gps_avoid_waypoints: " + stored_online_gps_avoid_waypoints.Count);
            for (int i = 0; i < stored_online_gps_avoid_waypoints.Count; i++)
                returnSB.AppendLine(" stored_online_gps_avoid_waypoints[" + i + "]: " + stored_online_gps_avoid_waypoints[i].ToString());

            returnSB.AppendLine(" stored_special_job: " + stored_special_job);
            returnSB.AppendLine(" police_ctrl: " + police_ctrl);

            returnSB.AppendLine(" stored_map_state: " + stored_map_state.ToString());
            returnSB.AppendLine(" stored_gas_pump_money: " + stored_gas_pump_money.ToString());

            returnSB.AppendLine(" stored_weather_change_timer: " + stored_weather_change_timer.ToString());

            returnSB.AppendLine(" stored_current_weather: " + stored_current_weather.ToString());
            returnSB.AppendLine(" stored_rain_wetness: " + stored_rain_wetness.ToString());
            returnSB.AppendLine(" time_zone: " + time_zone.ToString());

            returnSB.AppendLine(" time_zone_name: " + time_zone_name);

            returnSB.AppendLine(" last_ferry_position: " + last_ferry_position.ToString());

            returnSB.AppendLine(" stored_show_weigh: " + stored_show_weigh.ToString().ToLower());
            returnSB.AppendLine(" stored_need_to_weigh: " + stored_need_to_weigh.ToString().ToLower());

            returnSB.AppendLine(" stored_nav_start_pos: " + stored_nav_start_pos.ToString());
            returnSB.AppendLine(" stored_nav_end_pos: " + stored_nav_end_pos.ToString());

            returnSB.AppendLine(" stored_gps_behind: " + stored_gps_behind.Count);
            for (int i = 0; i < stored_gps_behind.Count; i++)
                returnSB.AppendLine(" stored_gps_behind[" + i + "]: " + stored_gps_behind[i].ToString());

            returnSB.AppendLine(" stored_gps_ahead: " + stored_gps_ahead.Count);
            for (int i = 0; i < stored_gps_ahead.Count; i++)
                returnSB.AppendLine(" stored_gps_ahead[" + i + "]: " + stored_gps_ahead[i].ToString());

            returnSB.AppendLine(" stored_gps_behind_waypoints: " + stored_gps_behind_waypoints.Count);
            for (int i = 0; i < stored_gps_behind_waypoints.Count; i++)
                returnSB.AppendLine(" stored_gps_behind_waypoints[" + i + "]: " + stored_gps_behind_waypoints[i].ToString());

            returnSB.AppendLine(" stored_gps_ahead_waypoints: " + stored_gps_ahead_waypoints.Count);
            for (int i = 0; i < stored_gps_ahead_waypoints.Count; i++)
                returnSB.AppendLine(" stored_gps_ahead_waypoints[" + i + "]: " + stored_gps_ahead_waypoints[i].ToString());

            returnSB.AppendLine(" stored_gps_avoid_waypoints: " + stored_gps_avoid_waypoints.Count);
            for (int i = 0; i < stored_gps_avoid_waypoints.Count; i++)
                returnSB.AppendLine(" stored_gps_avoid_waypoints[" + i + "]: " + stored_gps_avoid_waypoints[i].ToString());

            returnSB.AppendLine(" stored_tutorial_state: " + stored_tutorial_state.ToString());

            returnSB.AppendLine(" stored_map_actions: " + stored_map_actions.Count);
            for (int i = 0; i < stored_map_actions.Count; i++)
                returnSB.AppendLine(" stored_map_actions[" + i + "]: " + stored_map_actions[i]);

            returnSB.AppendLine(" clean_distance_counter: " + clean_distance_counter.ToString());
            returnSB.AppendLine(" clean_distance_max: " + clean_distance_max.ToString());
            returnSB.AppendLine(" no_cargo_damage_distance_counter: " + no_cargo_damage_distance_counter.ToString());
            returnSB.AppendLine(" no_cargo_damage_distance_max: " + no_cargo_damage_distance_max.ToString());
            returnSB.AppendLine(" no_violation_distance_counter: " + no_violation_distance_counter.ToString());
            returnSB.AppendLine(" no_violation_distance_max: " + no_violation_distance_max.ToString());

            returnSB.AppendLine(" total_real_time: " + total_real_time.ToString());
            returnSB.AppendLine(" real_time_seconds: " + real_time_seconds.ToString());

            returnSB.AppendLine(" visited_cities: " + visited_cities.Count);
            for (int i = 0; i < visited_cities.Count; i++)
                returnSB.AppendLine(" visited_cities[" + i + "]: " + visited_cities[i]);

            returnSB.AppendLine(" visited_cities_count: " + visited_cities_count.Count);
            for (int i = 0; i < visited_cities_count.Count; i++)
                returnSB.AppendLine(" visited_cities_count[" + i + "]: " + visited_cities_count[i].ToString());

            returnSB.AppendLine(" last_visited_city: " + last_visited_city);

            returnSB.AppendLine(" discovered_cutscene_items: " + discovered_cutscene_items.Count);
            for (int i = 0; i < discovered_cutscene_items.Count; i++)
                returnSB.AppendLine(" discovered_cutscene_items[" + i + "]: " + discovered_cutscene_items[i].ToString());

            returnSB.AppendLine(" discovered_cutscene_items_states: " + discovered_cutscene_items_states.Count);
            for (int i = 0; i < discovered_cutscene_items_states.Count; i++)
                returnSB.AppendLine(" discovered_cutscene_items_states[" + i + "]: " + discovered_cutscene_items_states[i].ToString());

            returnSB.AppendLine(" unlocked_dealers: " + unlocked_dealers.Count);
            for (int i = 0; i < unlocked_dealers.Count; i++)
                returnSB.AppendLine(" unlocked_dealers[" + i + "]: " + unlocked_dealers[i]);

            returnSB.AppendLine(" unlocked_recruitments: " + unlocked_recruitments.Count);
            for (int i = 0; i < unlocked_recruitments.Count; i++)
                returnSB.AppendLine(" unlocked_recruitments[" + i + "]: " + unlocked_recruitments[i]);

            returnSB.AppendLine(" total_screeshot_count: " + total_screeshot_count.ToString());

            returnSB.AppendLine(" undamaged_cargo_row: " + undamaged_cargo_row.ToString());

            returnSB.AppendLine(" service_visit_count: " + service_visit_count.ToString());
            returnSB.AppendLine(" last_service_pos: " + last_service_pos.ToString());

            returnSB.AppendLine(" gas_station_visit_count: " + gas_station_visit_count.ToString());
            returnSB.AppendLine(" last_gas_station_pos: " + last_gas_station_pos.ToString());

            returnSB.AppendLine(" emergency_call_count: " + emergency_call_count.ToString());
            returnSB.AppendLine(" ai_crash_count: " + ai_crash_count.ToString());

            returnSB.AppendLine(" truck_color_change_count: " + truck_color_change_count.ToString());

            returnSB.AppendLine(" red_light_fine_count: " + red_light_fine_count.ToString());

            returnSB.AppendLine(" cancelled_job_count: " + cancelled_job_count.ToString());

            returnSB.AppendLine(" total_fuel_litres: " + total_fuel_litres.ToString());
            returnSB.AppendLine(" total_fuel_price: " + total_fuel_price.ToString());

            returnSB.AppendLine(" transported_cargo_types: " + transported_cargo_types.Count);
            for (int i = 0; i < transported_cargo_types.Count; i++)
                returnSB.AppendLine(" transported_cargo_types[" + i + "]: " + transported_cargo_types[i]);

            returnSB.AppendLine(" achieved_feats: " + achieved_feats.ToString());
            returnSB.AppendLine(" discovered_roads: " + discovered_roads.ToString());

            returnSB.AppendLine(" discovered_items: " + discovered_items.Count);
            for (int i = 0; i < discovered_items.Count; i++)
                returnSB.AppendLine(" discovered_items[" + i + "]: " + discovered_items[i].ToString());

            returnSB.AppendLine(" drivers_offer: " + drivers_offer.Count);
            for (int i = 0; i < drivers_offer.Count; i++)
                returnSB.AppendLine(" drivers_offer[" + i + "]: " + drivers_offer[i]);

            returnSB.AppendLine(" freelance_truck_offer: " + freelance_truck_offer);

            returnSB.AppendLine(" trucks_bought_online: " + trucks_bought_online.ToString());
            returnSB.AppendLine(" special_cargo_timer: " + special_cargo_timer.ToString());

            returnSB.AppendLine(" screen_access_list: " + screen_access_list.Count);
            for (int i = 0; i < screen_access_list.Count; i++)
                returnSB.AppendLine(" screen_access_list[" + i + "]: " + screen_access_list[i]);

            returnSB.AppendLine(" driver_pool: " + driver_pool.Count);
            for (int i = 0; i < driver_pool.Count; i++)
                returnSB.AppendLine(" driver_pool[" + i + "]: " + driver_pool[i]);

            returnSB.AppendLine(" registry: " + registry);

            returnSB.AppendLine(" company_jobs_invitation_sent: " + company_jobs_invitation_sent.ToString().ToLower());

            returnSB.AppendLine(" company_check_hash: " + company_check_hash.ToString());

            returnSB.AppendLine(" relations: " + relations.Count);
            for (int i = 0; i < relations.Count; i++)
                returnSB.AppendLine(" relations[" + i + "]: " + relations[i].ToString());

            returnSB.AppendLine(" bus_stops: " + bus_stops.Count);
            for (int i = 0; i < bus_stops.Count; i++)
                returnSB.AppendLine(" bus_stops[" + i + "]: " + bus_stops[i]);

            returnSB.AppendLine(" bus_job_log: " + bus_job_log);

            returnSB.AppendLine(" bus_experience_points: " + bus_experience_points.ToString());

            returnSB.AppendLine(" bus_total_distance: " + bus_total_distance.ToString());

            returnSB.AppendLine(" bus_finished_job_count: " + bus_finished_job_count.ToString());
            returnSB.AppendLine(" bus_cancelled_job_count: " + bus_cancelled_job_count.ToString());

            returnSB.AppendLine(" bus_total_passengers: " + bus_total_passengers.ToString());
            returnSB.AppendLine(" bus_total_stops: " + bus_total_stops.ToString());

            returnSB.AppendLine(" bus_game_time: " + bus_game_time.ToString());
            returnSB.AppendLine(" bus_playing_time: " + bus_playing_time.ToString());

            returnSB.AppendLine("}");

            returnString = returnSB.ToString();

            return returnString;
        }

        //Methods Support
        internal void setPlayerSkillsArray()
        {
            playerSkills = new byte[6] { adr, long_dist, heavy, fragile, urgent, mechanical};
        }

        internal void getPlayerSkillsFromArray()
        {
            adr = playerSkills[0];
            long_dist = playerSkills[1];
            heavy = playerSkills[2];
            fragile = playerSkills[3];
            urgent = playerSkills[4];
            mechanical = playerSkills[5];
        }

        internal int[] getPlayerLvl()
        {
            int currentLvl = 0, lvlThreshhold = 0, 
                finalThreshhold = Globals.PlayerLevelUps[Globals.PlayerLevelUps.Length - 1];

            foreach (int lvlstep in Globals.PlayerLevelUps)
            {
                lvlThreshhold += lvlstep;

                if (experience_points < lvlThreshhold)
                    return new int[] { currentLvl, lvlThreshhold };
                else
                    currentLvl++;
            }

            do
            {
                lvlThreshhold += finalThreshhold;

                if (experience_points < lvlThreshhold)
                    return new int[] { currentLvl, lvlThreshhold };
                else
                    currentLvl++;

            } while (true);
        }

        internal void setPlayerExp(int _plLvl)
        {
            uint experience = 0;

            if (_plLvl < 0)
                _plLvl = 0;

            if (_plLvl > 150)
                _plLvl = 150;

            for (int i = 0; i < _plLvl; i++)
            {
                if (i < Globals.PlayerLevelUps.Length)
                    experience += (uint)Globals.PlayerLevelUps[i];
                else
                    experience += (uint)Globals.PlayerLevelUps[Globals.PlayerLevelUps.Length - 1];
            }

            if (_plLvl < Globals.PlayerLevelUps.Length)
                experience += (uint)Globals.PlayerLevelUps[_plLvl] - 1;
            else
                experience += (uint)Globals.PlayerLevelUps[Globals.PlayerLevelUps.Length - 1] - 1;

            experience_points = experience;
        }

    }
}
