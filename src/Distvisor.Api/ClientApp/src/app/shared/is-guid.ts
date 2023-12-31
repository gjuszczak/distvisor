export function isGuid(guid: any) : boolean {
    const validator = new RegExp("^[a-z0-9]{8}-[a-z0-9]{4}-[a-z0-9]{4}-[a-z0-9]{4}-[a-z0-9]{12}$", "i");
    return guid && validator.test(guid.toString());
}