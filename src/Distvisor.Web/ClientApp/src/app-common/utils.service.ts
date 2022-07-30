import { Injectable } from '@angular/core';

@Injectable({ providedIn: 'root' })
export class UtilsService {

  diff(obj1: any, obj2: any): any {
    if (!obj2 || Object.prototype.toString.call(obj2) !== '[object Object]') {
      return obj1;
    }
    let diffs: any = {};
    let key: string;

    let arraysMatch = (arr1: any, arr2: any): boolean => {
      if (arr1.length !== arr2.length) return false;
      for (let i = 0; i < arr1.length; i++) {
        if (arr1[i] !== arr2[i]) return false;
      }
      return true;
    };

    let compare = (item1: any, item2: any, key: string): void => {
      let type1 = Object.prototype.toString.call(item1);
      let type2 = Object.prototype.toString.call(item2);
      if (type2 === '[object Undefined]') {
        diffs[key] = null;
        return;
      }
      if (type1 !== type2) {
        diffs[key] = item2;
        return;
      }
      if (type1 === '[object Object]') {
        let objDiff = this.diff(item1, item2);
        if (Object.keys(objDiff).length > 0) {
          diffs[key] = objDiff;
        }
        return;
      }
      if (type1 === '[object Array]') {
        if (!arraysMatch(item1, item2)) {
          diffs[key] = item2;
        }
        return;
      }
      if (type1 === '[object Function]') {
        if (item1.toString() !== item2.toString()) {
          diffs[key] = item2;
        }
      } else {
        if (item1 !== item2) {
          diffs[key] = item2;
        }
      }
    };

    for (key in obj1) {
      if (obj1.hasOwnProperty(key)) {
        compare(obj1[key], obj2[key], key);
      }
    }
    for (key in obj2) {
      if (obj2.hasOwnProperty(key)) {
        if (!obj1[key] && obj1[key] !== obj2[key]) {
          diffs[key] = obj2[key];
        }
      }
    }

    return diffs;
  }
}
