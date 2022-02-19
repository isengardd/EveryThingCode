import _Vue from "vue"; // <-- notice the changed import
import { httpService, handlerHttpError } from "@/utilities/http";
/* eslint-disable @typescript-eslint/no-explicit-any */
// eslint-disable-next-line
export function GlobalPlugin(Vue: typeof _Vue, options?: any): void {
  // export type PluginFunction<T> = (Vue: typeof _Vue, options?: T) => void;
  Vue.prototype.$http = httpService;
  Vue.prototype.handlerHttpError = handlerHttpError;
  // UI
  Vue.prototype.pgTotalVisible = 9;
  // rules
  Vue.prototype.nameRules = [(v: string) => !!v || "Username is required"];
  Vue.prototype.passwordRules = [(v: string) => !!v || "Password is required"];
  Vue.prototype.minLengthRules = () => (minLen: number) => [(v: string) => (!!v && v.length >= minLen) || `最少${minLen}个字符`];
  Vue.prototype.digitRules = () => (testStr: string) => [() => !/.*[^0-9]/.test(testStr) || "只能输入非负数字"];
}
/* eslint-enable */
