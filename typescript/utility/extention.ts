// interface String {
//   format(...replacements: any[]): string;
// }

// String.prototype.format = function() {
//   let args = arguments;
//   return this.replace(/{(\d+)}/g, function(match, number) {
//     return typeof args[number] != "undefined" ? args[number] : match;
//   });
// };

// export function FormatString(str: string, ...args: string[]): string {
//   return str.replace(/{(\d+)}/g, function(match, number) {
//     return typeof args[number] != "undefined" ? args[number] : match;
//   });
// }

export class String {
  static format(str: string, ...args: string[]): string {
    return str.replace(/{(\d+)}/g, function(match, number) {
      return typeof args[number] !== "undefined" ? args[number] : match;
    });
  }
}

// vee-validate类型兼容
export type VForm = Vue & { validate: () => boolean; reset: () => void };

/*eslint-disable @typescript-eslint/no-explicit-any */
export const deepCopy = <T>(target: T): T => {
  if (target === null) {
    return target;
  }
  if (target instanceof Date) {
    return new Date(target.getTime()) as any;
  }
  if (target instanceof Array) {
    const cp = [] as any[];
    (target as any[]).forEach(v => {
      cp.push(deepCopy<any>(v));
    });
    return cp as any;
  }
  if (target instanceof Set) {
    const cp = new Set();
    (target as Set<any>).forEach(v => {
      cp.add(deepCopy(v));
    });
    return cp as any;
  }
  if (typeof target === "object" && target !== {}) {
    //const cp = { ...(target as { [key: string]: any }) } as { [key: string]: any };
    const cp: Record<string, any> = {};
    Object.entries(target).forEach(([k, v]) => {
      cp[k] = deepCopy<any>(v);
    });
    return cp as T;
  }
  return target;
};
/*eslint-enable */

/*eslint-disable @typescript-eslint/no-explicit-any */
export function objectsHaveSameKeys(...objects: Record<string, any>[]): boolean {
  if (objects.length < 2) {
    return true;
  }
  const keysArray: string[][] = new Array<string[]>(objects.length);
  let objKeyLength = -1;
  const allKeys: Set<string> = new Set();
  for (const [index, obj] of objects.entries()) {
    const objKeys = Object.keys(obj);
    if (objKeyLength >= 0) {
      if (objKeys.length !== objKeyLength) {
        return false;
      }
    } else {
      objKeyLength = objKeys.length;
    }
    keysArray[index] = objKeys;
  }
  keysArray.forEach(keys => {
    keys.forEach(key => {
      allKeys.add(key);
    });
    if (objKeyLength !== allKeys.size) {
      return false;
    }
  });
  return true;
  //const allKeys: string[] = objects.reduce<string[]>((keys: string[], object: Record<string,any>) => keys.concat(Object.keys(object)), []);
  //const union = new Set(allKeys);
  //return objects.every(object => union.size === Object.keys(object).length);
}
/*eslint-enable */

/*eslint-disable @typescript-eslint/no-explicit-any */
// 比较两个对象是否相同
export function deepCompare(a: any, b: any): boolean {
  if (typeof a !== typeof b) {
    return false;
  }

  if (a instanceof Date) {
    if (b instanceof Date) {
      if (a.getTime() !== b.getTime()) {
        return false;
      }
      return true;
    } else {
      return false;
    }
  }

  if (a instanceof Array) {
    if (b instanceof Array) {
      if (a.length !== b.length) {
        return false;
      }
      for (const [index, val] of a.entries()) {
        if (!deepCompare(val, b[index])) {
          return false;
        }
      }
      return true;
    } else {
      return false;
    }
  }

  if (a instanceof Set) {
    if (b instanceof Set) {
      if (a.size !== b.size) {
        return false;
      }
      const union = new Set([...a, ...b]);
      if (union.size !== a.size) {
        return false;
      }
      return true;
    } else {
      return false;
    }
  }

  if (typeof a === "object") {
    if (!objectsHaveSameKeys(a, b)) {
      return false;
    }
    for (const [key, value] of Object.entries(a)) {
      if (!deepCompare(value, (b as Record<string, any>)[key])) {
        return false;
      }
    }
    return true;
  }
  return a === b;
}

// getPropertyName(person, o => o.address);
export function getPropertyName<T>(obj: T, expression: (inst:T)=>any):string {
  var res = Object();
  Object.keys(obj).map(k => { res[k] = () => k; });
  return expression(res)();
}

/*eslint-enable */
