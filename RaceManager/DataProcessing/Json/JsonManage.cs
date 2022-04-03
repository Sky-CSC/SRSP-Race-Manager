﻿using RaceManager.DataProcessing;
using RaceManager.Communication;

namespace RaceManager.DataProcessing.Json
{
    public class JsonManage
    {
        private static RMLogger _logger = new(LoggingLevel.DEBUG, "JsonManage");
        public static string JsonType(string data)
        {
            dynamic informationJson = JsonParse.JsonDeserialize(data);
            Console.WriteLine(informationJson);
            _logger.log(LoggingLevel.DEBUG, "JsonType", informationJson);
            //try
            //{

            //}
            //catch (Exception ex)
            //{

            //}
            string OutMessage;
            _logger.log(LoggingLevel.DEBUG, "JsonType", $"TypeMessage: {(IMessageType)informationJson.TypeMessage}");
            switch ((IMessageType)informationJson.TypeMessage)
            {
                case IMessageType.CONNECTION:
                    _logger.log(LoggingLevel.DEBUG, "JsonType", informationJson.TypeMessage + " Connection");
                    OutMessage = JsonParse.JsonSerialiseOConnection();
                    break;
                case IMessageType.DISCONNECTION:
                    _logger.log(LoggingLevel.DEBUG, "JsonType", informationJson.TypeMessage + " Disconnection");
                    OutMessage = JsonParse.JsonSerialiseODisconnection();
                    break;
                case IMessageType.PLAYERINFO:
                    _logger.log(LoggingLevel.DEBUG, "JsonType", informationJson.TypeMessage + " PlayerInfo");
                    OutMessage = "Player Info";
                    break;
                case IMessageType.BOATSELECT:
                    _logger.log(LoggingLevel.DEBUG, "JsonType", informationJson.TypeMessage + " BoatSelect");
                    OutMessage = "Boat Select";
                    break;
                case IMessageType.BOATLISTREQUEST: //Retourner une liste de bateau
                    _logger.log(LoggingLevel.DEBUG, "JsonType", informationJson.TypeMessage + " BoatListRequest");
                    OutMessage = JsonParse.JsonSerialiseOBoatList();
                    //Envoyer à la personne qui m'as envoyé ma demande
                    break;
                case IMessageType.ENDRACE:
                    _logger.log(LoggingLevel.DEBUG, "JsonType", informationJson.TypeMessage + " EndRace");
                    OutMessage = JsonParse.JsonSerialiseOEndRace();
                    break;

                default:
                    OutMessage = "Error";
                    break;
            }
            return OutMessage;
        }
    }
}