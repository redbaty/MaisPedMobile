using SGE32.DAL.EnterpriseModels;

namespace MaisPedMobile.Com
{
    public static class PersonExtension
    {
        public static Person ToPerson(this Pessoa pessoa)
        {
            return new Person
            {
                Id = pessoa.Pessoacodigo,
                Name = pessoa.Pessoanome
            };
        }
    }
}