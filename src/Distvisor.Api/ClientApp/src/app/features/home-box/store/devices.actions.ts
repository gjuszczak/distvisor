import {
    DevicesListDto,
} from 'src/app/api/models';

export class LoadDevices {
    static readonly type = '[Home Box] LoadDevices';
    constructor(
        public readonly first?: number,
        public readonly rows?: number
    ) { }
}

export class LoadDevicesSuccess {
    static readonly type = '[Home Box] LoadDevicesSuccess';
    constructor(
        public readonly devices: DevicesListDto
    ) { }
}

export class LoadDevicesFail {
    static readonly type = '[Home Box] LoadDevicesFail';
    constructor(
        public readonly error: string 
    ) { }
}

export class SyncDevicesWithGateway {
    static readonly type = '[Home Box] SyncDevicesWithGateway';
}