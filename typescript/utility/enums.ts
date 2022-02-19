export const GlobalProductId = 0; // 全局配置的产品ID
// 登陆状态
export enum AuthState {
  AuthNone = 0,
  AuthLoggingIn = 1,
  AuthLoggedIn = 2,
}

// 用户等级
export enum UserLevel {
  UserVisitor = 0, // 访客
  UserPartner = 1, // 合作伙伴
  UserInsider = 2, // 普通成员
  UserAdmin = 3, // 管理员
}

// 用户操作行为
export enum UserAction {
  ActionRead = 0, // 0 查看
  ActionConfig = 1 << 0, // 1 << 0 修改配置相关
  ActionProduct = 1 << 1, // 1 << 1 修改项目相关
  ActionPublish = 1 << 2, // 1 << 2 发布
  ActionUser = 1 << 3, // 1 << 3 修改用户相关
}

// cache类型
export enum LocalStorageType {
  LocalStorageSession = "adcenter-user-session",
  LocalStorageUserBasic = "adcenter-user-basic",
  LocalStorageSelectedProduct = "adcenter-selected-product",
}

// 需要用到的服务端配置
export enum ServerCfgKey {
  ImageUrlBase = "ad.image.urlbase",
}

// 发布任务状态
export enum PublishTaskState {
  Publishing = "publishing",
  Failed = "failed",
  Completed = "completed",
}

// 发布状态
export enum AdPublishStatus {
  AdStatusUnknown = 0, // 未知
  AdStatusNoPub = 1, // 未投放
  AdStatusActive = 2, // 投放中
  AdStatusClose = 3, // 已关闭
}

// 错误信息枚举
export enum Error {
  ErrorNoErr = -1, // 没有错误
  ErrorUnknown = 0, //
  ErrorInner, // 内部错误
  ErrorNotImplement, // 接口未实现
  ErrorParams, // 参数错误
  ErrorDBConnect, // 数据库连接失败
  ErrorDBExecute, // 数据库执行错误
  ErrorDBNoData, // 数据不存在
  ErrorUserPassword, // 用户名密码错误
  ErrorLoginTooMuch, // 登录过于频繁
  ErrorAuthorityParse, // 权限解析失败
  ErrorNoAuthority, // 没有操作权限
  ErrorSessionExpire, // SESSION过期
  ErrorSessionError, // SESSION错误
  ErrorSessionNoExist, // SESSION不存在
  ErrorAddUserExist, // 添加的用户名已存在
  ErrorSaveImageFail, // 保存图片失败
}
