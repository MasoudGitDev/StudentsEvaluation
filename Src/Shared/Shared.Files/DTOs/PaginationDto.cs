namespace Shared.Files.DTOs;
public record PaginationDto(bool UsePagination = true , int PageNumber = 1 , int PageSize = 50);
