using Database.Models;
using Database.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckWeigherFood.Controls
{
  public partial class AppCore
  {
    private OperationSettingService _operationSettingService { get; set; }
    private AppConfigService _appConfigService { get; set; }
    private ProductService _productService { get; set; }
    private void ResgisterService()
    {
      _operationSettingService = AppFactory.CreateOperationSettingService();
      _appConfigService = AppFactory.CreateAppConfigService();
      _productService = AppFactory.CreateProductService();
    }

    public async Task UpdateAppConfig(AppConfig appConfig)
    {
      await _appConfigService.UpdateAsync(appConfig);
    }


  }
}
