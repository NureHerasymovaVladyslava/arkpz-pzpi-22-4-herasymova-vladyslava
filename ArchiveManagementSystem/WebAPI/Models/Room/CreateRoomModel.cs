﻿namespace WebAPI.Models.Room
{
    public class CreateRoomModel
    {
        public float TempMax { get; set; }
        public float TempMin { get; set; }
        public float HumMax { get; set; }
        public float HumMin { get; set; }
        public int LightMax { get; set; }
        public int LightMin { get; set; }
    }
}