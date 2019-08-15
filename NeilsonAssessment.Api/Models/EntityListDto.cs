using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using NeilsonAssessment.Api.Helpers;

namespace NeilsonAssessment.Api.Models
{
    public class EntityListDto<TDto, TEntity> : LinkedResourceBaseDto<ListLinksDto>
    {
        private readonly IUrlHelper _urlHelper;
        public List<TDto> Items { get; set; }

        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int PerPage { get; set; }
        public int TotalCount { get; set; }

        public EntityListDto(PagedList<TEntity> pagedList, IUrlHelper urlHelper, string routeName)
        {
            _urlHelper = urlHelper;

            CurrentPage = pagedList.CurrentPage;
            TotalPages = pagedList.TotalPages;
            PerPage = pagedList.PerPage;
            TotalCount = pagedList.TotalCount;
            Items = AutoMapper.Mapper.Map<List<TDto>>(pagedList);

            PopulateLinks(routeName);
        }

        private void PopulateLinks(string routeName)
        {
            _links = new ListLinksDto();

            var selfUrl = _urlHelper.Link(routeName, new { page = CurrentPage, perPage = PerPage });
            if (selfUrl != null)
            {
                _links.Self = new LinkDto(selfUrl, "self", "GET");
            }

            var previousUrl = (CurrentPage > 1) ? _urlHelper.Link(routeName, new { page = CurrentPage - 1, perPage = PerPage }) : null;
            if (previousUrl != null)
            {
                _links.Prev = new LinkDto(previousUrl, "prev_page", "GET");
            }

            var nextUrl = (CurrentPage < TotalPages) ? _urlHelper.Link(routeName, new { page = CurrentPage + 1, perPage = PerPage }) : null;
            if (nextUrl != null)
            {
                _links.Next = new LinkDto(nextUrl, "next_page", "GET");
            }

            var firstUrl = (CurrentPage > 1) ? _urlHelper.Link(routeName, new { page = 1, perPage = PerPage }) : null;
            if (firstUrl != null)
            {
                _links.First = new LinkDto(firstUrl, "first_page", "GET");
            }

            var lastUrl = (CurrentPage < TotalPages) ? _urlHelper.Link(routeName, new { page = TotalPages, perPage = PerPage }) : null;
            if (lastUrl != null)
            {
                _links.Last = new LinkDto(lastUrl, "last_page", "GET");
            }
        }
    }
}
