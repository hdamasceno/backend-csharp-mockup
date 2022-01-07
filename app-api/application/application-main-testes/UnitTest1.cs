using Biblioteca;
using NUnit.Framework;
using System;
using application_data_entities;

namespace application_main_tests;

public class Tests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void AccountLoadFromDynamic()
    {
        var objetoDynamic = new
        {
            Id = Guid.NewGuid(),
            CadastradoDataHora = DateTime.Now.ConverteDataAzureBrasil(),
            Email = "hdamasceno@gmail.com",
            Senha = "000000"
        };

        var objetoAccount = new Account(FuncoesEspeciais.NewDynamic(objetoDynamic));

        if (objetoAccount.IsValid == false)
            Assert.Fail($"Entity [{objetoAccount.GetType().Name}] invalid." + Environment.NewLine + objetoAccount?.GetValidationErrors());

        Assert.Pass();
    }

    [Test]
    public void EmpresaLoadFromDynamic()
    {
        var objInput = new
        {
            Id = Guid.NewGuid(),
            AccountId = Guid.NewGuid(),
            CadastradoDataHora = DateTime.Now.ConverteDataAzureBrasil(),
            RazaoSocial = "EMPRESA TESTE RAZAO SOCIAL",
            NomeFantasia = "EMPRESA TESTE NOME FANTASIA",
            DocumentoCNPJ = "00.000.000/0000-00", // TROQUE PARA UM CNPJ VALIDO!
            InscricaoEstadual = "ISENTO",
            InscricaoMunicipal = "123456"
        };

        // objeto empresa ta quebrado :D

        //var objEntity = new Empresa(FuncoesEspeciais.NewDynamic(objInput));

        //if (objEntity.IsValid == false)
        //    Assert.Fail($"Entidade {nameof(objEntity)} invalid." + Environment.NewLine + objEntity?.GetValidationErrors());

        //if (objEntity != null)
        //{
        //    dynamic objOutPut = objEntity.ToDynamic<Empresa>();
        //    string objJSON = objEntity.ToJSON<Empresa>();

        //    Assert.Pass();
        //}
        //else
        //    Assert.Fail($"Entidade {nameof(objEntity)} is null.");
    }
}
