﻿using RaceManager.DataProcessing;

namespace RaceManager.DataProcessing.Json
{
    public class JsonManage
    {

        public static void JsonType(string data)
        {
            dynamic informationJson = JsonParse.JsonDeserialize(data);
            Console.WriteLine(informationJson);
            try
            {

            }
            catch (Exception ex)
            {

            }

            switch (informationJson.TypeMessage)
            {
                case 0:

                case 1:

                case 2:

                case 3:

                default:
                    break;
            }
        }
    }
}