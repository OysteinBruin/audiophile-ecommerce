export interface Result<T> {
    succeeded: boolean;
    failed: boolean;
    message: string;
    data: T | null;
}