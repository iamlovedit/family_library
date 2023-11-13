import service from "@/utils/request";

export function getPackageList(pageIndex: number, pageSize: number) {
    const url = `/package/v1/packages?pageIndex=${pageIndex}&pageSize=${pageSize}`
    return service.get(url)
}