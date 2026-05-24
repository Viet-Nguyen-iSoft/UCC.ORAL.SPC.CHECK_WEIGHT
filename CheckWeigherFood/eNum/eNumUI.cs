using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckWeigherFood.eNum
{
  public class eNumUI
  {
    public enum AppModulSupport
    {
      DashBoard,
      MasterData,
      Synthetic,
      Report,
      Setting,
      Synchronized,
    }

    public enum eProductCheck
    {
      BOX,
      BOTTLE,
      ITEM,
    }

    public enum eResgisterStart
    {
      OPName = 100,
      QCName = 120,
      TCName = 140,
      FGs = 160,
      LoBB = 170,
      NameProduct = 180,
      SKU = 230,
      Lower2T = 240,
      Lower1T = 250,
      Target = 260,
      Upper1T = 270,
      Upper2T = 280,
      ProductId = 290,


    }
    public enum eLengthResgister
    {
      OPName = 20,
      QCName = 20,
      TCName = 20,
      FGs = 10,
      LoBB = 10,
      NameProduct = 50,
      SKU = 10,
      Lower2T = 10,
      Lower1T = 10,
      Target = 10,
      Upper1T = 10,
      Upper2T = 10,
      ProductId = 5,
    }


    public enum eImage
    {
      SeePass,
      NoSeePass,
      Confirm,
      Question,
      Warning,
      Information,
    }

    public enum eSetting
    {
      DateSafe,
      DateQuality,
      OLE,
      OR,
      LoBB,
      OP, 
      QC,
      TC,
    }

    public enum eStatus
    {
      Reject = 0,
      OK = 1,
      Over = 2,
    }



  }
}
