﻿using RaceManager.Communication;
using RaceManager.DataProcessing;
using RaceManager.DataProcessing.Json;

// See https://aka.ms/new-console-template for more information

class ProgrammeServer
{
    public static int Main(String[] args)
    {
        Console.WriteLine("Hello, World!");

        //IMessageType TypeMessage = 0;
        //long Id = 4242;
        //long IdGame = 01;
        //string NMEA = "YOLO";
        //long Boat = 0;
        //int IdPlayer = 123;
        //string NamePlayer = "Sky";

        //string test = JsonParse.JsonSerialiseConnection(TypeMessage, Id, IdGame, NMEA, Boat, IdPlayer, NamePlayer);
        //Console.WriteLine("test" + test);

        //JsonManage.JsonType(test);

        string[] str = new string[] { "a", "b" };

        AsyncServer.Port = 45678;
        AsyncServer.Run();


        Console.WriteLine();
        return 0;
    }
}