using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Umove_AS.Models
{
    internal class Block
    {
        DateTime blockStart;
        DateTime blockEnd;
        int blockID;
        int blockLength;
        int blockPause;
        int routeLenghtInKm;
        int blockTotalDistanceInKm;
        int blockTotalTimeInService;
        int blockTotalTime;
    }
}
