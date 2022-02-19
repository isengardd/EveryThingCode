import axios from "axios";
import { vxm } from "@/store";
import { ToastPosition } from "@/store/modules/toast";
import { ToastConfig, ServerError } from "./imp_proto";

// http
export const httpService = axios.create({
  baseURL: process.env.VUE_APP_API_ROOT.replace("HOSTNAME", window.location.hostname),
  headers: { "Content-Type": "application/json" },
});

/* eslint-disable @typescript-eslint/no-explicit-any */
export function handlerHttpError(error: any, msgErr: string, pos: number = ToastPosition.CenterTop) {
  if (error.response) {
    const svrError = new ServerError(error.response.data);
    vxm.toast.error(
      new ToastConfig({ msg: `请求状态:${error.response.status} - ${msgErr} - 错误 ${svrError.code}: ${svrError.message}`, pos: pos })
    );
  } else {
    vxm.toast.error(new ToastConfig({ msg: `${error}`, pos: pos }));
  }
}
/* eslint-enable */
export default httpService;
