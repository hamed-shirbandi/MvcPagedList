using MvcPagedList.Example.Enums;
using MvcPagedList.Example.Service.Users.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcPagedList.Example.Service.Users
{
    public interface IUserService
    {
        IEnumerable<UserOutput> Search(int page, int recordsPerPage, string term, SortBy sortBy, SortOrder sortOrder, out int pageSize, out int TotalItemCount);
        void Create(UserInput input);
    }
}