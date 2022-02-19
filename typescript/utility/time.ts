export function getNowTime(): number {
  return Math.floor(new Date().getTime() / 1000);
}

export function getNowTimeMS(): number {
  return new Date().getTime();
}

export function sleep(ms: number) {
  return new Promise(resolve => setTimeout(resolve, ms));
}

export function secondToHour(sec: number): string {
  return `${Math.floor(sec / 3600)}:${Math.floor((sec % 3600) / 60)}:${sec % 60}`;
}
