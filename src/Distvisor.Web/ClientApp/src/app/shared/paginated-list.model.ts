export interface PaginatedList<T> {
    items: T[],
    first: number,
    rows: number,
    rowsPerPageOptions: number[]
    totalRecords: number,
    loading: boolean,
    error: string,
}