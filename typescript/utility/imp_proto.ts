import { UserLevel, Error, AdPublishStatus } from "@/utilities/enums";
import { ToastPosition } from "@/store/modules/toast";

export class ServerError {
  public code = Error.ErrorNoErr;
  public message = "";

  public constructor(init?: Partial<ServerError>) {
    Object.assign(this, init);
  }
}

export class UserProductAuthority {
  public id = 0; // 项目ID
  public right = 0; // 权限 UserAction

  public constructor(init?: Partial<UserProductAuthority>) {
    Object.assign(this, init);
  }
}

export class UserAuthority {
  public all = 0; // 是否所有项目可见
  public ban: number[] = []; // 不可见项目ID
  public auth: UserProductAuthority[] = []; // 具体项目的权限

  public constructor(init?: Partial<UserAuthority>) {
    Object.assign(this, init);
    this.auth = [];
    if (init?.auth) {
      init?.auth.forEach(value => {
        this.auth.push(new UserProductAuthority(value));
      });
    }
    // 兼容服务端数组信息返回null的情况
    if (!init?.ban) {
      this.ban = [];
    }
  }
}

export class LoginRequest {
  public userName = "";
  public password = "";

  public constructor(init?: Partial<LoginRequest>) {
    Object.assign(this, init);
  }
}

export class ToastConfig {
  public msg = "";
  public timeout = 3000; // 默认展示3秒
  public pos: number = ToastPosition.CenterTop;

  public constructor(init?: Partial<ToastConfig>) {
    Object.assign(this, init);
  }
}

export class AdConfig {
  public id = 0;
  public key = "";
  public value = "";
  public desc = "";
  public sort = 0;

  public constructor(init?: Partial<AdConfig>) {
    Object.assign(this, init);
  }
}

export class UserBasic {
  public userName = "";
  public nickName = "";
  public level: number = UserLevel.UserVisitor;
  public authority: UserAuthority = new UserAuthority();
  public desc = "";

  public constructor(init?: Partial<UserBasic>) {
    Object.assign(this, init);
    if (init?.authority) {
      this.authority = new UserAuthority(init?.authority);
    }
  }
}

export class Pagination {
  public count = 0;
  public perPage = 10;
  public curPage = 1;
  public totalPages = 1;

  public constructor(init?: Partial<Pagination>) {
    Object.assign(this, init);
  }
}

export class ConfigsPage {
  public data: AdConfig[] = [];
  public pagination: Pagination = new Pagination();

  public constructor(init?: Partial<ConfigsPage>) {
    Object.assign(this, init);
    if (init?.data) {
      this.data = [];
      init?.data.forEach(value => {
        this.data.push(new AdConfig(value));
      });
    }
    // 数组为null的处理
    if (!init?.data) {
      this.data = [];
    }
    if (init?.pagination) {
      this.pagination = new Pagination(init.pagination);
    }
  }
}

export class AdProduct {
  public id = 0;
  public name = "";
  public icon = "";
  public fbLocalConfigPath = ""; // fb广告投放配置文档（excel）的本地路径
  public fbLocalResPath = ""; // fb广告视频资源的本地路径
  public fbAdAcctId = ""; // fb广告投放账号
  public shortName = ""; // 产品名缩写
  public fbAppId = ""; // fb应用id
  public fbPageId = ""; // fb主页id
  public fbInsId = ""; // instagram id
  public fbExcludeAudience = ""; // 用于排除已安装用户的自定义受众ID
  public fbStoreURLAndroid = ""; // android商店url
  public fbStoreURLIos = ""; // ios商店Url
  public fbAccessToken = "";

  public constructor(init?: Partial<AdProduct>) {
    Object.assign(this, init);
  }
}

export class ProductsPage {
  public data: AdProduct[] = [];
  public pagination: Pagination = new Pagination();

  public constructor(init?: Partial<ProductsPage>) {
    Object.assign(this, init);
    if (init?.data) {
      this.data = [];
      init?.data.forEach(value => {
        this.data.push(new AdProduct(value));
      });
    }
    if (!init?.data) {
      this.data = [];
    }
    if (init?.pagination) {
      this.pagination = new Pagination(init.pagination);
    }
  }
}

// 广告投放配置
export class AdServingConfig {
  public id = 0;
  public platform = "";
  public campaignID = "empty";
  public campaign = "";
  public adSet = "";
  public country = "";
  public adUnit = "";
  public dynamicCreative = 0;
  public videoDirs: string[] = [];
  public videoNames: string[] = [];
  public adContents: string[] = [];
  public adTitles: string[] = [];
  public ageMin = "";
  public status = AdPublishStatus.AdStatusUnknown; // 投放状态
  public constructor(init?: Partial<AdServingConfig>) {
    Object.assign(this, init);
  }
}

export class AdServingConfigPage {
  public data: AdServingConfig[] = [];
  public pagination: Pagination = new Pagination();

  public constructor(init?: Partial<ProductsPage>) {
    Object.assign(this, init);
    if (init?.data) {
      this.data = [];
      let id = 1;
      init?.data.forEach(value => {
        const newConfig = new AdServingConfig(value);
        newConfig.id = id;
        this.data.push(newConfig);
        id++;
      });
    }
    if (!init?.data) {
      this.data = [];
    }
    if (init?.pagination) {
      this.pagination = new Pagination(init.pagination);
    }
  }
}

export class TaskInfo {
  public taskID = "";
  public msg = "";
  public state = "";
  public constructor(init?: Partial<TaskInfo>) {
    Object.assign(this, init);
  }
}

export class AudienceInfo {
  public id = "";
  public name = "";
  public country = "";
  public constructor(init?: Partial<AudienceInfo>) {
    Object.assign(this, init);
  }
}

export class AudienceArray {
  public audiences: AudienceInfo[] = [];
  public constructor(init?: Partial<AudienceArray>) {
    Object.assign(this, init);
    this.audiences = [];
    if (init?.audiences) {
      init?.audiences.forEach(value => {
        const newAudience = new AudienceInfo(value);
        this.audiences.push(newAudience);
      });
    }
  }
}

export class BusinessUsage {
  public callCount = 0;
  public cpuTime = 0;
  public totalTime = 0;
  public constructor(init?: Partial<BusinessUsage>) {
    Object.assign(this, init);
  }
}

export class AdActUsage {
  public accUtilPct = 0;
  public constructor(init?: Partial<AdActUsage>) {
    Object.assign(this, init);
  }
}

export class AppUsage {
  public callCount = 0;
  public cpuTime = 0;
  public totalTime = 0;
  public constructor(init?: Partial<AppUsage>) {
    Object.assign(this, init);
  }
}

export class FbApiUsage {
  public businessList: BusinessUsage[] = [];
  public adAccount: AdActUsage = new AdActUsage();
  public app: AppUsage = new AppUsage();
  public regainTime = 0; // 剩余解锁时间（秒）
  public constructor(init?: Partial<FbApiUsage>) {
    Object.assign(this, init);
    this.businessList = [];
    if (init?.businessList) {
      init?.businessList.forEach(value => {
        const newBusiness = new BusinessUsage(value);
        this.businessList.push(newBusiness);
      });
    }
    if (init?.adAccount) {
      this.adAccount = new AdActUsage(init.adAccount);
    }
    if (init?.app) {
      this.app = new AppUsage(init.app);
    }
  }
}

export class UnpublishCountryInfo {
  public countryCode = "";
  public countryName = "";
  public languageCode = "";
  public languageName = "";
  public adContent = "";
  public adTitle = "";
  public constructor(init?: Partial<UnpublishCountryInfo>) {
    Object.assign(this, init);
  }
}
