using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FixedPUCB;

namespace FixedRP
{
    public class ArtworkService
    {
        public async Task<Result> ChangeOwner(Artwork artwork, string newOwnerId)
        {
            if (artwork == null)
            {
                //Logging logic
                return new Result(false);
            }
            if (string.IsNullOrEmpty(newOwnerId))
            {
                //Logging logic
                return new Result(false);
            }

            try
            {
                artwork.OwnerId = newOwnerId;

                //DB logic
                //Logging logic

                return new Result(true);
            }
            catch (Exception ex)
            {
                //Logging logic
                return new Result(false);
            }
        }
    }

    public class Artwork
    {
        public string OwnerId { get; set; }
    }
}
