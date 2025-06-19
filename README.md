<<<<<<< HEAD
# Guiando TWM - Monitoramento de E-mail Iguatemi

Este projeto implementa uma **Azure Function** escrita em C# para monitorar uma caixa de e-mail específica do Iguatemi, processar anexos de faturas recebidas e realizar integração com o sistema TWM.

---

## 🚀 Tecnologias Utilizadas

- C# (.NET)
- Azure Functions SDK
- Integração via REST com TWM
- Configuração via `local.settings.json` (Azure Functions local)

---

## 📁 Estrutura do Projeto

- `IntegracaoIguatemi.cs`: Contém a lógica principal da função que acessa o e-mail, processa os anexos e envia os dados ao sistema.
- `Startup.cs`: Inicialização de dependências e configuração da função Azure.
- `host.json`: Configuração padrão de host do Azure Functions.
- `local.settings.json`: Contém as variáveis de ambiente usadas localmente.
- `*.csproj`: Arquivo de definição do projeto com dependências NuGet.
- `.git/`: Diretório com metadados do repositório Git.

---

## 🛠️ Como Executar Localmente

### Requisitos:
- .NET SDK instalado
- Azure Functions Core Tools
- Conta de e-mail e configurações de autenticação

### Passos:

1. Configure seu `local.settings.json` com as credenciais de acesso ao e-mail e configurações da API TWM.
2. Compile e execute localmente com:

```bash
func start
```

> Isso iniciará a função e ela ficará escutando por novos eventos (como chegada de e-mails).

---

## ⚙️ Observações

- A função é configurada para rodar via **timer trigger** ou escutar diretórios (dependendo da implementação atual).
- Os logs são fundamentais para validar o fluxo de envio.

---

## 📄 Licença

MIT
=======
# AzureFunctionMonitoraEmail
Azure Function em C# responsável por monitorar uma caixa de e-mail, extrair anexos de faturas e integrá-los ao sistema TWM de forma automática e segura.
>>>>>>> 1f2d8805263a72a6b618fe90ee83e000f0ef2020
