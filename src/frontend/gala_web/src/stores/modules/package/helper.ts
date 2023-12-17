export type Package = {
    name: string
    createdDate: string
    updatedDate: string
    description: string
    id: string
    downloads: number
    votes: number
    versions: PackageVersion[]
}

export type PackageVersion = {
    version: string
    createdDate: string
}