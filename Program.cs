using System.IO;
using System.Text;

class Ingrediente
{
    private string nome;
    private int quantidade;
    private string unidade;

    public Ingrediente(string nome, int quantidade, string unidade)
    {
        this.nome = nome;
        this.quantidade = quantidade;
        this.unidade = unidade;
    }

    public string Nome
    {
        get { return nome; }
        set { nome = value; }
    }

    public int Quantidade
    {
        get { return quantidade; }
        set { quantidade = value; }
    }

    public string Unidade
    {
        get { return unidade; }
        set { unidade = value; }
    }

    public override string ToString()
    {
        return $"{nome} - {quantidade}{unidade}";
    }
}

class Receita
{
    public List<Ingrediente> Ingredientes = new List<Ingrediente>();
    Ingrediente? IngredienteAtual;
    private string? nome, nomeIngrediente;
    private string? tipoIngrediente;
    private double precoReceita;
    int quantidadeservida;

    public double PrecoReceita
    {
        get { return precoReceita; }
        set { precoReceita = value; }
    }
      
     public string Nome
    {
        get { return nome!; }
        set { nome = value; }
    }

    public void Adicionar()
    {
        string Resp;
        Console.WriteLine("Qual o nome da receita?");
        nome = Console.ReadLine()!;
        Console.WriteLine("Para quantas pessoas ela será servida?");
        quantidadeservida = Convert.ToInt32(Console.ReadLine());
        Console.WriteLine("Quanto custará essa receita?");
        precoReceita = Convert.ToDouble(Console.ReadLine());
        precoReceita = (precoReceita / quantidadeservida);
        do
        {
            Console.WriteLine("Insira o nome do ingrediente:");
            nomeIngrediente = Console.ReadLine()!;
            Console.WriteLine("Qual a medida do ingrediente?");
            Console.WriteLine("1-grama(g)\n2-mililitros(ml)\n3-unidade(u)");
            Resp = Console.ReadLine()!;
            switch (Resp)
            {
                case "1":
                    tipoIngrediente = "g";
                    break;
                case "2":
                    tipoIngrediente = "ml";
                    break;
                case "3":
                    tipoIngrediente = "u";
                    break;
            }
            Console.WriteLine("Insira a quantidade utilizada do ingrediente:");
            int quantidadeIngrediente = Convert.ToInt32(Console.ReadLine());
            quantidadeIngrediente = (quantidadeIngrediente / quantidadeservida);
            IngredienteAtual = new Ingrediente(nomeIngrediente, quantidadeIngrediente, tipoIngrediente!);
            Ingredientes.Add(IngredienteAtual);
            Console.WriteLine("Deseja adicionar mais um ingrediente? [S/N]");
            Resp = Console.ReadLine()!.ToUpper();
        } while (Resp != "N" && Resp != "NAO");
    }
}

class Arquivos
{
    static public void ArquivosLer(List<Receita> Receitas)
    {
        string CaminhoDesktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        string CaminhoArquivo = Path.Combine(CaminhoDesktop, "DadosReceita.txt");
        if (!File.Exists(CaminhoArquivo))
        {
            StreamWriter Criar = File.CreateText(CaminhoArquivo);
            Criar.Close();
        }
        StreamReader Ler = new StreamReader(File.OpenRead(CaminhoArquivo));
        string linha;
        while ((linha = Ler.ReadLine()!) != null)
        {
            string[] Dados = linha.Split(';');
            if (Dados[0] == "RCT")
            {
                Receitas.Add(new Receita { Nome = Dados[1], PrecoReceita = double.Parse(Dados[2]) });
            }
            else
            {
                Receitas.Last().Ingredientes.Add(new Ingrediente(Dados[1], int.Parse(Dados[2]), Dados[3]));
            }
        }
        Ler.Close();
    }

    static public void ArquivosSalvar(List<Receita> Receitas)
    {
        string CaminhoDesktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        string CaminhoArquivo = Path.Combine(CaminhoDesktop, "DadosReceita.txt");
        StreamWriter Escrever = new StreamWriter(CaminhoArquivo, false, Encoding.UTF8);
        foreach (var receita in Receitas)
        {
            Escrever.WriteLine($"RCT;{receita.Nome};{receita.PrecoReceita}");
            foreach (var ingrediente in receita.Ingredientes)
            {
                Escrever.WriteLine($"ING;{ingrediente.Nome};{ingrediente.Quantidade};{ingrediente.Unidade}");
            }
        }
        Escrever.Close();
    }
}

class Util
{
    static public void Mostrar(List<Receita> Receitas)
    {
        int Cont = 1;
        foreach (Receita receita in Receitas)
        {
            Console.WriteLine($"{Cont}: {receita.Nome}");
            Cont++;
        }
    }

    static public void PrepararReceita(Receita ReceitaAtual,double QuantidadePessoas)
    {
        Console.WriteLine($"{ReceitaAtual.Nome} para {QuantidadePessoas} pessoa(s):");
        foreach(var Ingrediente in ReceitaAtual.Ingredientes)
        {
            Console.WriteLine($"{Ingrediente.Nome} - {Math.Round((Ingrediente.Quantidade * QuantidadePessoas),2)}{Ingrediente.Unidade}");
        }
        Console.WriteLine($"Valor: R${Math.Round((ReceitaAtual.PrecoReceita * QuantidadePessoas), 2)}");
    }
}

class Program
{
    static void Main(string[] args)
    {
        List<Receita> Receitas = new List<Receita>();
        Arquivos.ArquivosLer(Receitas);
        //Antes do resto do codigo ler as receitas atuais
        Receita receitaatual = new Receita();
        int Resp;
        int EscolhaReceita;
        do
        {
            Console.Clear();
            Console.WriteLine("|----------------------|");
            Console.WriteLine("|---------MENU---------|");
            Console.WriteLine("|----------------------|");
            Console.WriteLine("|  1-Preparar receita  |");
            Console.WriteLine("|  2-Ver receitas      |");
            Console.WriteLine("|  3-Adicionar receita |");
            Console.WriteLine("|  4-Remover receita   |");
            Console.WriteLine("|       5-Sair         |");
            Console.WriteLine("|______________________|");
            Resp = Convert.ToInt32(Console.ReadLine());
            Console.Clear();
            switch (Resp)
            {
                case 1:
                    Console.WriteLine("Qual receita deseja preparar ?"); 
                    Util.Mostrar(Receitas);
                    EscolhaReceita = Convert.ToInt32(Console.ReadLine());
                    Console.WriteLine("Para quantas pessoas será essa receita?");
                    int QuantidadePessoasPreparo = Convert.ToInt32(Console.ReadLine());
                    Util.PrepararReceita(Receitas[EscolhaReceita - 1], QuantidadePessoasPreparo);
                    Console.ReadKey();
                    break;
                case 2:
                    Util.Mostrar(Receitas);
                    Console.WriteLine("Pressiona qualquer tecla para continuar");
                    Console.ReadKey();
                    break;
                case 3:
                    Receitas.Add(new Receita());
                    Receitas[Receitas.Count - 1].Adicionar();
                    Console.WriteLine("Receita adicionada com sucesso! Pressiona qualquer tecla para continuar");
                    Console.ReadKey();
                    break;
                case 4:
                    Console.WriteLine("Qual receita deseja excluir?");
                    Util.Mostrar(Receitas);
                    EscolhaReceita = Convert.ToInt32(Console.ReadLine());
                    Receitas.RemoveAt(EscolhaReceita - 1);
                    Console.WriteLine("Receita removida com sucesso! Pressiona qualquer tecla para continuar");
                    Console.ReadKey();
                    break;
            }
        } while (Resp != 5);
        Arquivos.ArquivosSalvar(Receitas);
    }
}