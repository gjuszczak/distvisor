export interface PaginatedList<T> {
    items: T[],
    firstOffset: number,
    pageSize: number,
    pageSizeOptions: number[]
    totalCount: number,
    loading: boolean
}