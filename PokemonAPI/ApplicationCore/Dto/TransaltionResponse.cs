using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PokemonAPI.ApplicationCore.Dto
{
    public class TransaltionResponse
    {
        public SuccessResponse Success { get; set; }
        public Content Contents { get; set; }

        public class SuccessResponse
        {
            public int Total { get; set; }
        }
        public class Content
        {
            public string Translated { get; set; }
        }
    }
}
