using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using NeilsonAssessment.Api.Helpers;

namespace NeilsonAssessment.Api.Models
{
    public class CarListDto: LinkedResourceBaseDto<ListLinksDto>
    {
        private readonly IUrlHelper _urlHelper;
        public List<CarDto> Items { get; set; }

        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int PerPage { get; set; }
        public int TotalCount { get; set; }

        public CarListDto(PagedList<Entities.Car> pagedList, IUrlHelper urlHelper)
        {
            _urlHelper = urlHelper;

            CurrentPage = pagedList.CurrentPage;
            TotalPages = pagedList.TotalPages;
            PerPage = pagedList.PerPage;
            TotalCount = pagedList.TotalCount;
            Items = AutoMapper.Mapper.Map <List<CarDto>>(pagedList);

            Items.ForEach(f => f.PopulateLinks(urlHelper));
            PopulateLinks();
        }

        private void PopulateLinks()
        {
            _links = new ListLinksDto();

            var selfUrl = _urlHelper.Link("GetCars", new { page = CurrentPage, perPage = PerPage });
            if (selfUrl != null)
            {
                _links.Self = new LinkDto(selfUrl, "self", "GET");
            }

            var previousUrl = (CurrentPage > 1) ? _urlHelper.Link("GetCars", new { page = CurrentPage - 1, perPage = PerPage }) : null;
            if (previousUrl != null)
            {
                _links.Prev = new LinkDto(previousUrl, "prev_page", "GET");
            }

            var nextUrl = (CurrentPage < TotalPages) ? _urlHelper.Link("GetCars", new { page = CurrentPage + 1, perPage = PerPage }) : null;
            if (nextUrl != null)
            {
                _links.Next = new LinkDto(nextUrl, "next_page", "GET");
            }

            var firstUrl = (CurrentPage > 1) ? _urlHelper.Link("GetCars", new { page = 1, perPage = PerPage }) : null;
            if (firstUrl != null)
            {
                _links.First = new LinkDto(firstUrl, "first_page", "GET");
            }

            var lastUrl = (CurrentPage < TotalPages) ? _urlHelper.Link("GetCars", new { page = TotalPages, perPage = PerPage }) : null;
            if (lastUrl != null)
            {
                _links.Last = new LinkDto(lastUrl, "last_page", "GET");
            }
        }
    }
}
