using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Data;
using TCC_SAMMI.Api;
using Dapper;

public interface IUsuarioService
{
    List<Usuario> returnUsuarios();
    Usuario returnUsuario(int id);
    Usuario returnUsuariobyNome(string nome);
    bool addUsuario(Usuario user);
    Usuario updateUsuario(string email, Usuario user);
    void deleteUsuario(int id);
}

public class UsuarioService : IUsuarioService
{
    private readonly IConfiguration _config;
    //private readonly string connectionString = "Server=localhost:3306;Database=sammi_games;Uid=root;Pwd='';";
    private IDbConnection conn = new MySqlConnection("Server=localhost;Database=sammi_games;Uid=root;Pwd='';");
    public UsuarioService(IConfiguration config)
    {
        _config = config;
    }

    public List<Usuario> returnUsuarios()
    {
       conn.Open();
       return conn.Query<Usuario>("SELECT * FROM tbusuarios WHERE status = 'ativo'").ToList();
    }

    public Usuario returnUsuario(int id)
    {
        conn.Open();
        return conn.QueryFirstOrDefault<Usuario>("SELECT * FROM tbusuarios WHERE cod = @Id", new { Id = id });
    }

    public Usuario returnUsuariobyNome(string nome)
    {
        conn.Open();
        return conn.QueryFirstOrDefault<Usuario>("SELECT * FROM tbusuarios WHERE nome = @nome", new { nome = nome });
    }

    public bool addUsuario(Usuario user)
    {
        conn.Open();
        int result = conn.Execute("INSERT INTO tbusuarios (nome, email, senha, adi, sub, mul, divisao, silabas, datacriacao, status) VALUES (@nome, @email, @senha, 0, 0, 0, 0, 0, NOW(), 'ativo')", user);
        return result > 0 ? true : false;
    }

    public Usuario updateUsuario(string email, Usuario user)
    {
       conn.Open();
       conn.Execute("UPDATE tbusuarios SET nome = @nome, email = @email, senha = @senha, adi = @adi, sub = @sub, mul = @mul, divisao = @div, silabas = @silabas, datacriacao = @datacriacao, status = @status WHERE email = @email", new { Id = email, nome = user.nome, email = user.email, senha = user.senha, adi = user.adi, sub = user.sub, mul = user.mul, div = user.div, silabas = user.silabas, datacriacao = user.datacriacao, status = user.status });
       return conn.QueryFirstOrDefault<Usuario>("SELECT * FROM tbusuarios WHERE email = @email", new { email = email });
    }

    public void deleteUsuario(int id)
    {
       conn.Open();
       conn.Execute("UPDATE tbusuarios SET status = 'inativo' WHERE cod = @Id", new { Id = id }); 
    }
}
