using ListaDeTarefa.Application.DTOs.Pessoa;
using ListaDeTarefa.Domain.Entities;
using ListaDeTarefa.Domain.Enumerables;

namespace ListaDeTarefa.Application.Factories
{
	public static class PessoaFactory
	{
		public static PessoaDto EntidadeParaDto(Pessoa pessoa) => new()
		{
			Id = pessoa.Id,
			Nome = pessoa.Nome,
			Sobrenome = pessoa.Sobrenome,
			Email = pessoa.Email,
			Telefone = pessoa.Telefone,
			WhatsApp = pessoa.WhatsApp,
			DataNascimento = pessoa.DataNascimento
		};

		public static Pessoa CreateDtoParaEntidade(PessoaCreateDto dto) =>
			Pessoa.Criar(dto.Nome, dto.Sobrenome, dto.Email, dto.Telefone, dto.WhatsApp, dto.DataNascimento, TipoPessoa.Cliente);
	}
}
