import { defineStore } from 'pinia'
import { getPackageList } from '@/api/package'

export const usePackageStore = defineStore('package', () => {

    function getPackages(pageIndex: number, pageSize: number=20) {
        return getPackageList(pageIndex, pageSize)
    }
    return {
        getPackages
    }
})