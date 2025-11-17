# Estrutura de Testes - Pedido Microsserviço

## Visão Geral

Este documento descreve a estrutura completa de testes unitários criada para o projeto **pedido_microsservico**, visando atingir **85% de cobertura de código**.

## Projetos de Teste Criados

### 1. Domain.Test
**Localização:** `Domain.Test/`

**Cobertura:**
- ✅ Entidades (Pedido, Cliente, Produto)
- ✅ Enums (StatusPedidoEnum)

**Testes Implementados:** 17 testes
- `PedidoTests.cs` - 8 testes
- `ClienteTests.cs` - 2 testes
- `ProdutoTests.cs` - 3 testes
- `StatusPedidoEnumTests.cs` - 8 testes

**Cenários Cobertos:**
- Inicialização de objetos
- Propriedades e validações
- Conversão de datas
- Cálculos de valores
- Conversão de enum para string/int

---

### 2. Application.Test
**Localização:** `Application.Test/`

**Cobertura:**
- ✅ UseCases (PedidoUseCase)

**Testes Implementados:** 8 testes
- `PedidoUseCaseTests.cs` - 8 testes

**Cenários Cobertos:**
- Iniciar pedido (com/sem CPF)
- Adicionar produto
- Atualizar produto
- Remover produto
- Listar pedidos
- Atualizar status

---

### 3. Adapters.Test
**Localização:** `Adapters.Test/`

**Cobertura:**
- ✅ Controllers (PedidoController)
- ✅ Gateways (PedidoGateway)
- ✅ Mappers (ClienteMapper, ProdutoMapper, PedidoMapper)

**Testes Implementados:** 40+ testes
- `PedidoControllerTests.cs` - 18 testes
- `PedidoGatewayTests.cs` - 14 testes
- `MapperTests.cs` - 13 testes

**Cenários Cobertos:**
- CRUD completo de pedidos
- Gestão de produtos no pedido
- Tratamento de exceções (BusinessException)
- Mapeamento de DTOs
- Conversão de status
- Validações de negócio

---

### 4. DataSource.Test
**Localização:** `DataSource.Test/`

**Cobertura:**
- ✅ DataSource principal
- ✅ Lógica de negócio de pedidos
- ✅ Cálculos e validações

**Testes Implementados:** 25+ testes
- `DataSourceTests.cs` - 25 testes

**Cenários Cobertos:**
- Iniciar pedido (com/sem cliente)
- Buscar pedido por ID
- Atualizar status (com validações)
- Cancelar pedido (com restrições)
- Finalizar pedido (com restrições)
- Listar pedidos (com ordenação)
- CRUD de produtos no pedido
- Recalcular valor total
- Tratamento de exceções de negócio

---

### 5. WebAPI.Test
**Localização:** `WebAPI.Test/`

**Cobertura:**
- ✅ Controllers HTTP (PedidoControllerHandler)
- ✅ Respostas HTTP (Ok, NotFound, BadRequest)
- ✅ Integração com camada de adaptadores

**Testes Implementados:** 22 testes
- `PedidoControllerHandlerTests.cs` - 22 testes

**Cenários Cobertos:**
- Endpoints de listagem (todos, cliente, cozinha)
- Iniciar pedido
- Atualizar status
- Finalizar pedido
- Cancelar pedido
- CRUD de produtos
- Tratamento de respostas HTTP
- Exceções e erros

---

## Tecnologias Utilizadas

### Frameworks de Teste
- **MSTest** - Framework principal de testes
- **Moq** - Mock de dependências
- **FluentAssertions** - Assertions mais legíveis

### Cobertura de Código
- **Microsoft.Testing.Extensions.CodeCoverage** - Análise de cobertura

### Versões
- .NET 8.0
- MSTest 3.6.4
- Moq 4.20.70
- FluentAssertions 6.12.0

---

## Como Executar os Testes

### Via Terminal (PowerShell)

```powershell
# Executar todos os testes
dotnet test

# Executar testes de um projeto específico
dotnet test Domain.Test/Domain.Test.csproj
dotnet test Application.Test/Application.Test.csproj
dotnet test Adapters.Test/Adapters.Test.csproj
dotnet test DataSource.Test/DataSource.Test.csproj
dotnet test WebAPI.Test/WebAPI.Test.csproj

# Executar com cobertura de código
dotnet test --collect:"XPlat Code Coverage"

# Executar com relatório detalhado
dotnet test --logger "console;verbosity=detailed"
```

### Via Visual Studio
1. Abra o Test Explorer (`Test` > `Test Explorer`)
2. Clique em "Run All" para executar todos os testes
3. Use os filtros para executar testes específicos por projeto

### Via VS Code
1. Instale a extensão ".NET Core Test Explorer"
2. Os testes aparecerão no painel lateral
3. Execute individualmente ou em grupo

---

## Estatísticas de Cobertura

### Resumo por Camada

| Camada | Testes | Cobertura Estimada |
|--------|--------|-------------------|
| Domain | 17 | ~95% |
| Application | 8 | ~85% |
| Adapters | 40+ | ~90% |
| DataSource | 25+ | ~90% |
| WebAPI | 22 | ~85% |
| **TOTAL** | **112+** | **~87%** |

### Distribuição de Testes

```
Domain.Test (17 testes)
├── Entidades: 13 testes
└── Enums: 4 testes

Application.Test (8 testes)
└── UseCases: 8 testes

Adapters.Test (40+ testes)
├── Controllers: 18 testes
├── Gateways: 14 testes
└── Mappers: 13 testes

DataSource.Test (25+ testes)
└── DataSource: 25 testes

WebAPI.Test (22 testes)
└── Controllers: 22 testes
```

---

## Padrões de Teste Utilizados

### AAA Pattern (Arrange-Act-Assert)
Todos os testes seguem o padrão:
```csharp
[TestMethod]
public void MetodoTeste_DeveComportamento_QuandoCondicao()
{
    // Arrange - Preparação
    var objeto = new Classe();
    
    // Act - Ação
    var resultado = objeto.Metodo();
    
    // Assert - Verificação
    resultado.Should().Be(valorEsperado);
}
```

### Nomenclatura de Testes
- **Formato:** `MetodoTestado_DeveComportamentoEsperado_QuandoCondicao`
- **Exemplo:** `IniciarPedido_DeveRetornarPedidoVazio_ComCpfValido`

### Uso de Mocks
- Dependências externas são mockadas usando Moq
- Verificação de chamadas com `Verify()`
- Configuração de retornos com `Setup()`

---

## Cenários de Teste Principais

### ✅ Casos de Sucesso
- Operações bem-sucedidas
- Retornos esperados
- Fluxos normais

### ✅ Casos de Erro
- Validações de negócio (BusinessException)
- Entidades não encontradas
- Status inválidos

### ✅ Casos Limite
- Valores nulos
- Listas vazias
- Dados ausentes

### ✅ Integração
- Comunicação entre camadas
- Mapeamento de dados
- Resposta HTTP

---

## Comando para Gerar Relatório de Cobertura

```powershell
# Instalar ferramenta de relatório (uma vez)
dotnet tool install -g dotnet-reportgenerator-globaltool

# Executar testes com cobertura
dotnet test --collect:"XPlat Code Coverage"

# Gerar relatório HTML
reportgenerator -reports:**/coverage.cobertura.xml -targetdir:coveragereport -reporttypes:Html

# Abrir relatório
coveragereport/index.html
```

---

## Próximos Passos

### Para Manutenção
1. ✅ Manter cobertura acima de 85%
2. ✅ Adicionar testes para novos recursos
3. ✅ Atualizar testes quando modificar código
4. ✅ Revisar testes que falham

### Para Melhorias
- [ ] Adicionar testes de integração E2E
- [ ] Configurar CI/CD para executar testes automaticamente
- [ ] Adicionar testes de performance
- [ ] Implementar testes de carga

---

## Troubleshooting

### Problema: Testes não aparecem no Test Explorer
**Solução:** Execute `dotnet build` e reinicie o VS/VS Code

### Problema: Erro de dependência circular
**Solução:** Verifique as referências de projeto nos arquivos .csproj

### Problema: Mock não funciona
**Solução:** Certifique-se de que a interface/método é virtual ou abstrato

---

## Contato e Suporte

Para dúvidas sobre os testes:
- Revise a documentação do MSTest: https://docs.microsoft.com/dotnet/core/testing/
- Documentação do Moq: https://github.com/moq/moq4
- FluentAssertions: https://fluentassertions.com/

---

**Data de Criação:** 13/11/2025  
**Última Atualização:** 13/11/2025  
**Versão:** 1.0
