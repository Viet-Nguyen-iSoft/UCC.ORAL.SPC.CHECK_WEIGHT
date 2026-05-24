using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckWeigherFood.eNum
{
  public class eRegisterPLC
  {

    public enum eRegisterPLCStart
    {
      STTMax = 620,
      DayQuery = 622,
      BitRequestReadDataWeigher = 623,

      BuferSTT = 625,
      BuferValue = 627,
      BuferDay = 629,
      BuferHH = 630,
      BuferMM = 631,
      BuferSS = 632,




      OP = 800,
      QC = 820,
      TC = 840,
      FGs = 860,
      NameProduct = 870,
      LoBB = 930,

      NumberSafe = 940,
      NumberQuality = 942,
      OLE = 944,
      OR = 946,
      NormalSpeed = 950,
      OW = 952,
      NumberReject = 954,

      valueTempLow = 956,
      valueTempMid = 958,
      valueTempHigh = 960,
    }
    public enum eRegisterPLCLength
    {
      STTMax = 2,
      DayQuery = 1,
      BitRequestReadDataWeigher = 1,

      BuferSTT = 2,
      BuferValue = 2,
      BuferDay = 1,
      BuferHH = 1,
      BuferMM = 1,
      BuferSS = 1,


      OP = 20,
      QC = 20,
      TC = 20,
      FGs = 10,
      NameProduct = 60,
      LoBB = 10,
      OLE = 2,
      OR = 2,
      NormalSpeed = 2,
      OW = 2,
      NumberReject = 2,

      valueTempLow = 2,
      valueTempMid = 2,
      valueTempHigh = 2,

    }


  }
}
