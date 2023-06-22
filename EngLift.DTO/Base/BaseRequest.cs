﻿namespace EngLift.DTO.Base
{
    public class BaseRequest
    {
        private string? _search;
        public int Limit { get; set; } = 15;
        public int Page { get; set; } = 1;
        public string? Search { set { _search = value; } get { return _search?.ToLower(); } }
        public int? Sort { get; set; }//tự quy định
        public bool? Active { get; set; }
    }
}
