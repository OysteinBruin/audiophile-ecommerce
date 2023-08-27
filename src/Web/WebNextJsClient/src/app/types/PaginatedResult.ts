import { Result } from './Result';
export interface PaginatedResult<T> {
    succeeded: boolean;
    failed: boolean;
    message: string;
    currentPage: number;
    totalPages: number;
    totalCount: number;
    pageSize: number;
    hasPreviousPage: boolean;
    hasNextPage: boolean;
    data: T[] | null;
}