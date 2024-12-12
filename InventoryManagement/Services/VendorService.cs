using InventoryManagement.DTOs;
using InventoryManagement.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace InventoryManagement.Services
{
    public class VendorService
    {
        private readonly VendorRepository _vendorRepository;

        public VendorService(VendorRepository vendorRepository)
        {
            _vendorRepository = vendorRepository;
        }

        public async Task<IEnumerable<VendorDto>> GetVendorListAsync()
        {
            var vendors = await _vendorRepository.GetVendorList();
            return vendors.Select(v => new VendorDto
            {
                VendorID = v.VendorID,
                VendorName = v.VendorName,
                ContactNumber = v.ContactNumber,
                Address = v.Address
            }).ToList();    
        }
    }
}