using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
namespace Data.Services
{
    partial class DataServiceBase
    {
        public async Task<Dictionary<int, string>> GetTalukOptions()
        {
            return await _dataSource.Taluks.Where(x=>x.TalukIsActive).Select(x => new { x.TalukId, x.TalukName }).OrderBy(x=>x.TalukName).ToDictionaryAsync(t => t.TalukId, t => t.TalukName);
        }
        public async Task<Dictionary<int, string>> GetAllTalukOptions()
        {
            return await _dataSource.Taluks.Select(x => new { x.TalukId, x.TalukName }).OrderBy(x => x.TalukName).ToDictionaryAsync(t => t.TalukId, t => t.TalukName);
        }
        public async Task<Dictionary<int, string>> GetHobliOptions()
        {
            return await _dataSource.Hoblis.Where(x=>x.HobliIsActive).Select(x => new { x.HobliId, x.HobliName }).OrderBy(x => x.HobliName).ToDictionaryAsync(t => t.HobliId, t => t.HobliName);
        }
        public async Task<Dictionary<int, string>> GetAllHobliOptions()
        {
            return await _dataSource.Hoblis.Select(x => new { x.HobliId, x.HobliName }).OrderBy(x => x.HobliName).ToDictionaryAsync(t => t.HobliId, t => t.HobliName);
        }
        public async Task<Dictionary<int, string>> GetHobliOptionsByTaluk(int talukId)
        {
            return await _dataSource.Hoblis.Where(x=>x.HobliIsActive).Where(x=>x.TalukId==talukId).Select(x => new { x.HobliId, x.HobliName }).OrderBy(x => x.HobliName).ToDictionaryAsync(t => t.HobliId, t => t.HobliName);
        }
        public async Task<Dictionary<int, string>> GetVillageOptions()
        {
            return await _dataSource.Villages.Where(x=>x.VillageIsActive==true).Select(x => new { x.VillageId, x.VillageName }).OrderBy(x => x.VillageName).ToDictionaryAsync(t => t.VillageId, t => t.VillageName);
        }
        public async Task<Dictionary<int, string>> GetAllVillageOptions()
        {
            return await _dataSource.Villages.Select(x => new { x.VillageId, x.VillageName }).OrderBy(x => x.VillageName).ToDictionaryAsync(t => t.VillageId, t => t.VillageName);
        }

        public async Task<Dictionary<int, string>> GetVillageOptionsByHobli(int hobliId)
        {
            return await _dataSource.Villages.Where(x=>x.VillageIsActive==true).Where(x=>x.HobliId== hobliId).Select(x => new { x.VillageId, x.VillageName }).OrderBy(x => x.VillageName).ToDictionaryAsync(t => t.VillageId, t => t.VillageName);
        }
        public async Task<Dictionary<int, string>> GetCompanyOptions()
        {
            return await _dataSource.Companies.Where(x=>x.IsActive).Select(x => new { x.CompanyID, x.Name }).OrderBy(x => x.Name).ToDictionaryAsync(t => t.CompanyID, t => t.Name);
        }
        public async Task<Dictionary<int, string>> GetAllCompanyOptions()
        {
            return await _dataSource.Companies.Select(x => new { x.CompanyID, x.Name }).OrderBy(x => x.Name).ToDictionaryAsync(t => t.CompanyID, t => t.Name);
        }
        public async Task<Dictionary<int, string>> GetAccountTypeOptions()
        {
            return await _dataSource.AccountTypes.Select(x => new { x.AccountTypeId, x.AccountTypeName }).OrderBy(x => x.AccountTypeName).ToDictionaryAsync(t => t.AccountTypeId, t => t.AccountTypeName);
        }
        public async Task<Dictionary<int, string>> GetExpenseHeadOptions()
        {
            return await _dataSource.ExpenseHeads.Where(x=>x.IsExpenseHeadActive==true).Select(x => new { x.ExpenseHeadId, x.ExpenseHeadName }).OrderBy(x => x.ExpenseHeadName).ToDictionaryAsync(t => t.ExpenseHeadId, t => t.ExpenseHeadName);
        }
        public async Task<Dictionary<int, string>> GetPartyOptions()
        {
            return await _dataSource.Parties.Where(x=>x.IsPartyActive==true).Select(x => new { x.PartyId, x.PartyFirstName }).OrderBy(x => x.PartyFirstName).ToDictionaryAsync(t => t.PartyId, t => t.PartyFirstName);
        }

        public async Task<Dictionary<int, string>> GetPartyOptionsByProperty(int propertyId)
        {
            var models = await (from pp in _dataSource.PropertyParty.Where(x => x.PropertyId == propertyId)
                               from party in _dataSource.Parties.Where(x => x.PartyId == pp.PartyId)
                               select new
                               {                                   
                                   PartyId = party.PartyId,
                                   PartyName = party.PartyFirstName,
                               }).OrderBy(x => x.PartyName).ToDictionaryAsync(t => t.PartyId, t => t.PartyName);
            return models;
        }

        public async Task<Dictionary<int, string>> GetPartyOptionsByGroup(int groupId)
        {
            var models = await (from party in _dataSource.Parties.Where(x => x.GroupId == groupId )
                                select new
                                {
                                    PartyId = party.PartyId,
                                    PartyName = party.PartyFirstName,
                                }).OrderBy(x => x.PartyName).ToDictionaryAsync(t => t.PartyId, t => t.PartyName);
            return models;
        }


        public async Task<Dictionary<int, string>> GetAllDocumentTypeOptions()
        {
            return await _dataSource.DocumentTypes.Select(x => new { x.DocumentTypeId, x.DocumentTypeName }).OrderBy(x => x.DocumentTypeName).ToDictionaryAsync(t => t.DocumentTypeId, t => t.DocumentTypeName);
        }
        public async Task<Dictionary<int, string>> GetDocumentTypeOptions()
        {
            return await _dataSource.DocumentTypes.Where(x=>x.IsDocumentTypeActive).Select(x => new { x.DocumentTypeId, x.DocumentTypeName }).OrderBy(x => x.DocumentTypeName).ToDictionaryAsync(t => t.DocumentTypeId, t => t.DocumentTypeName);
        }

        public async Task<Dictionary<int, string>> GetPropertyOptions()
        {
            return await _dataSource.Properties.Select(x => new { x.PropertyId, x.PropertyName }).OrderBy(x => x.PropertyName).ToDictionaryAsync(t => t.PropertyId, t => t.PropertyName);
        }

        public async Task<Dictionary<int, string>> GetUnSoldPropertyOptions()
        {
            return await _dataSource.Properties.Where(x=>x.IsSold==false || x.IsSold==null).Select(x => new { x.PropertyId, x.PropertyName }).OrderBy(x => x.PropertyName).ToDictionaryAsync(t => t.PropertyId, t => t.PropertyName);
        }

        public async Task<Dictionary<int, string>> GetCashOptions()
        {
            return await _dataSource.CashAccounts.Where(x=>x.IsCashAccountActive==true).Select(x => new { x.CashAccountId, x.CashAccountName }).OrderBy(x => x.CashAccountName).ToDictionaryAsync(t => t.CashAccountId, t => t.CashAccountName);
        } 
        public async Task<Dictionary<int, string>> GetCashOptionsByCompany(int companyId)
        {
            return await _dataSource.CashAccounts.Where(x=>x.IsCashAccountActive==true).Where(x=>x.CompanyID==companyId).Select(x => new { x.CashAccountId, x.CashAccountName }).OrderBy(x => x.CashAccountName).ToDictionaryAsync(t => t.CashAccountId, t => t.CashAccountName);
        }
        public async Task<Dictionary<int, string>> GetBankOptions()
        {
            return await _dataSource.BankAccounts.Where(x=>x.IsBankAccountActive).Select(x => new { x.BankAccountId, x.BankName }).OrderBy(x => x.BankName).ToDictionaryAsync(t => t.BankAccountId, t => t.BankName);
        }
        public async Task<Dictionary<int, string>> GetBankOptionsByCompany(int companyId){
            return await _dataSource.BankAccounts.Where(x=>x.IsBankAccountActive).Where(x=>x.CompanyID==companyId).Select(x => new { x.BankAccountId, x.BankName ,x.AccountNumber}).OrderBy(x => x.BankName).ToDictionaryAsync(t => t.BankAccountId, t => t.BankName+" - "+t.AccountNumber);
        }
        public async Task<Dictionary<int, string>> GetVendorOptions()
        {
            return await _dataSource.Vendors.Where(x=>x.IsVendorActive==true).Select(x => new { x.VendorId, x.VendorName }).OrderBy(x => x.VendorName).ToDictionaryAsync(t => t.VendorId, t => t.VendorName);
        }


        public Dictionary<int, string> GetReportingToOptions()
        {
            Dictionary<int, string> list = new Dictionary<int, string>();
            list.Add(1, "Managers");
            list.Add(2, "Groups");
            return list;
        }
        public Dictionary<int, string> GetGenderOptions()
        {
            Dictionary<int, string> list = new Dictionary<int, string>();
            list.Add(1, "Male");
            list.Add(2, "Female");
            return list;
        }
        public Dictionary<int, string> GetGroupsTypeOptions()
        {
            Dictionary<int, string> list = new Dictionary<int, string>();
            list.Add(1, "Party");
            list.Add(2, "Vendor");
            return list;
        }
        public async Task<Dictionary<int, string>> GetGroupsOptions()
        {
            return await _dataSource.Groups.Select(x => new { x.GroupId, x.GroupName }).OrderBy(x => x.GroupName).ToDictionaryAsync(t => t.GroupId, t => t.GroupName);
        }
        public async Task<Dictionary<int, string>> GetGroupsOptionsForParty()
        {
            return await _dataSource.Groups.Where(x=>x.GroupType==1).Select(x => new { x.GroupId, x.GroupName }).OrderBy(x => x.GroupName).ToDictionaryAsync(t => t.GroupId, t => t.GroupName);
        }

        public async Task<Dictionary<int, string>> GetGroupsOptionsByProperty(int propertyId)
        {
            var item = _dataSource.PropertyParty.Where(x => x.PropertyId == propertyId).FirstOrDefault();
            if (item == null)
                return null;
            if (item.IsGroup)
                return await _dataSource.Groups.Where(x => x.GroupId == item.PartyId).Select(x => new { x.GroupId, x.GroupName }).OrderBy(x => x.GroupName).ToDictionaryAsync(t => t.GroupId, t => t.GroupName);
            else
                return null;
        }

        public async Task<Dictionary<int, string>> GetGroupsOptionsForVendor()
        {
            return await _dataSource.Groups.Where(x => x.GroupType == 2).Select(x => new { x.GroupId, x.GroupName }).OrderBy(x => x.GroupName).ToDictionaryAsync(t => t.GroupId, t => t.GroupName);
        }
        public async  Task<List<DropDownList>> GetPartyOptions(string party)
        {
            if (string.IsNullOrEmpty(party)) {
             return   await _dataSource.Groups.Where(x => x.GroupType == 1 )
                 .Select(x => new DropDownList { Id = x.GroupId, Description = x.GroupName + "(G)" }).OrderBy(x => x.Description).ToListAsync();
            }


            var Parties = await _dataSource.Parties.Where(x => x.PartyFirstName.Contains(party) && x.GroupId == 0)
                .Select(x => new DropDownList { Id = x.PartyId, Description = x.PartyFirstName}).OrderBy(x=>x.Description)
                .ToListAsync();

            var PartyGroups = await _dataSource.Groups.Where(x => x.GroupType == 1 && x.GroupName.Contains(party))
                 .Select(x => new DropDownList { Id = x.GroupId, Description =  x.GroupName+"(G)"}).OrderBy(x => x.Description).ToListAsync();
            if (PartyGroups.Count == 0)
            {
                PartyGroups = await (from g in _dataSource.Groups join  p in _dataSource.Parties on g.GroupId equals p.GroupId
                                     where p.PartyFirstName.Contains(party) && g.GroupType==1
                                     select new DropDownList { Id = g.GroupId, Description = g.GroupName + "(G)" }).ToListAsync();
            }

            if (Parties.Count == 0)
                return PartyGroups;
            else
                PartyGroups.ToList().ForEach(x => Parties.Add(x));
           return Parties;
        }

        public async Task<Dictionary<int, string>> GetPropertyTypeOptions()
        {
            return await _dataSource.PropertyTypes.Select(x => new { x.PropertyTypeId, x.PropertyTypeText }).OrderBy(x => x.PropertyTypeText).ToDictionaryAsync(t => t.PropertyTypeId, t => t.PropertyTypeText);
        }

        public async Task<Dictionary<int, string>> GetRoleOptions()
        {
            return await _dataSource.Roles.Select(x => new { x.RoleId, x.Name }).OrderBy(x => x.Name).ToDictionaryAsync(t => t.RoleId, t => t.Name);
        }

        public async Task<List<DropDownList>> GetVendorOptions(string vendor)
        {
            if (string.IsNullOrEmpty(vendor)) { 
            return await _dataSource.Groups.Where(x => x.GroupType == 2 )
                 .Select(x => new DropDownList { Id = x.GroupId, Description = x.GroupName + "(G)" }).OrderBy(x => x.Description).ToListAsync();
            }

            var vendors = await _dataSource.Vendors.Where(x => x.VendorName.Contains(vendor))
               .Select(x => new DropDownList { Id = x.VendorId, Description =  x.VendorName }).OrderBy(x => x.Description).ToListAsync();

            var vendorGroups = await _dataSource.Groups.Where(x => x.GroupType == 2 && x.GroupName.Contains(vendor))
                 .Select(x => new DropDownList { Id = x.GroupId, Description = x.GroupName+"(G)" }).OrderBy(x => x.Description).ToListAsync();
            
            if (vendorGroups.Count == 0){
                vendorGroups = await (from g in _dataSource.Groups
                                     join v in _dataSource.Vendors on g.GroupId equals v.GroupId
                                     where v.VendorName.Contains(vendor) && g.GroupType == 1
                                     select new DropDownList { Id = g.GroupId, Description = g.GroupName + "(G)" }).ToListAsync();
            }
            if (vendors.Count == 0)
                return vendorGroups;
            else
                vendorGroups.ForEach(x => vendors.Add(x));
            return vendors;
            //var vendors = await _dataSource.Vendors.Where(x => x.VendorName.Contains(vendor))
            //    .Select(x => new { x.VendorId, x.VendorName }).ToDictionaryAsync(t => t.VendorId.ToString(), t => "Vendor- " + t.VendorName);

            //var vendorGroups = await _dataSource.Groups.Where(x => x.GroupType == 2 && x.GroupName.Contains(vendor))
            //     .Select(x => new { x.GroupId, x.GroupName })
            //     .ToDictionaryAsync(t => t.GroupId.ToString(), t => "Group- " + t.GroupName);
            //if (vendors.Count == 0)
            //    return vendorGroups;
            //else
            //    vendorGroups.ToList().ForEach(x => vendors.Add(x.Key, x.Value));
            //return vendors;
            // return await _dataSource.Vendors.Where(x => x.VendorName.Contains(party)).Select(x => new { x.VendorId, x.VendorName }).ToDictionaryAsync(t => t.VendorId, t => t.VendorName);
        }

        public async Task<Dictionary<int, string>> GetCheckListOptions()
        {
            return await _dataSource.CheckLists.Select(x => new { x.CheckListId, x.CheckListName }).OrderBy(x => x.CheckListName).ToDictionaryAsync(t => t.CheckListId, t => t.CheckListName);
        }

        public async Task<Dictionary<int, string>> GetPropertyCheckListOptions()
        {
            return await _dataSource.PropertyCheckList.Select(x => new { x.PropertyCheckListId, x.PropertyName }).OrderBy(x => x.PropertyName).ToDictionaryAsync(t => t.PropertyCheckListId, t => t.PropertyName);
        }

        public async Task<Dictionary<int, string>> GetPropertyMergeOptions()
        {
            return await _dataSource.PropertyMerge.Where(x=>x.ForProposal==false).Select(x => new { x.PropertyMergeId, x.PropertyMergeDealName }).OrderBy(x => x.PropertyMergeDealName).ToDictionaryAsync(t => t.PropertyMergeId, t => t.PropertyMergeDealName);
        }

        public async Task<Dictionary<int, string>> GetPropertyOptionsByCompanyID(int companyId)
        {
            if(companyId==0)
            return await _dataSource.Properties.Select(x => new { x.PropertyId, x.PropertyName }).OrderBy(x => x.PropertyName).ToDictionaryAsync(t => t.PropertyId, t => t.PropertyName);
            else
            return await _dataSource.Properties.Where(x=>x.CompanyID==companyId).Select(x => new { x.PropertyId, x.PropertyName }).OrderBy(x => x.PropertyName).ToDictionaryAsync(t => t.PropertyId, t => t.PropertyName);
        }

        public async Task<Dictionary<int, string>> GetDocumentTypesByPropertyID(int propertyId)
        {           
            var items=await (from dp in _dataSource.PropertyDocumentType join
                      d in _dataSource.DocumentTypes on dp.DocumentTypeId equals d.DocumentTypeId
                      where dp.PropertyId== propertyId
                      select new { d.DocumentTypeId,d.DocumentTypeName}).OrderBy(x => x.DocumentTypeName).ToDictionaryAsync(t => t.DocumentTypeId, t => t.DocumentTypeName);

            return items;
        }

        public async Task<Dictionary<int, string>> GetDealPartiesOptions(int dealId)
        {
            var models = await (from dp in _dataSource.DealParties join p in _dataSource.Parties on dp.PartyId equals p.PartyId
                                where (dp.DealId == dealId)
                                select new { dp.PartyId, p.PartyFirstName }).OrderBy(x => x.PartyFirstName).ToDictionaryAsync(t => t.PartyId, t => t.PartyFirstName); 
            return models;
        }

        public async Task<Dictionary<int, string>> GetDealOptions()
        {
            return await (from d in _dataSource.Deal
                        join pm in _dataSource.PropertyMerge on d.PropertyMergeId equals pm.PropertyMergeId
                        select new { d.DealId, pm.PropertyMergeDealName }).OrderBy(x => x.PropertyMergeDealName).ToDictionaryAsync(t => t.DealId, t => t.PropertyMergeDealName);           
        }
        public async Task<Dictionary<int, string>> GetDealOptionsByCompany(int companyId)
        {
            return await (from d in _dataSource.Deal
                          join pm in _dataSource.PropertyMerge on d.PropertyMergeId equals pm.PropertyMergeId where d.CompanyId==companyId
                          select new { d.DealId, pm.PropertyMergeDealName }).OrderBy(x => x.PropertyMergeDealName).ToDictionaryAsync(t => t.DealId, t => t.PropertyMergeDealName);
        }

        public Dictionary<int, string> GetSalutationOptions()
        {
            Dictionary<int, string> list = new Dictionary<int, string>();
            list.Add(1, "Son of");
            list.Add(2, "Daughter of");
            list.Add(3, "Wife of");
            return list;
        }
    }
}
