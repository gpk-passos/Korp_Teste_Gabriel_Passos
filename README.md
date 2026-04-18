
Detalhamento Técnico da Solução - ERP Korp
Este documento descreve as escolhas técnicas e as ferramentas utilizadas no desenvolvimento do sistema de Estoque e Faturamento.

1. Ciclos de Vida do Angular
Utilizei o hook ngOnInit no componente principal. Ele funciona como o ponto de partida do sistema: assim que a tela é carregada, ele dispara a função carregarTudo(), que busca os dados iniciais de produtos e notas fiscais nos servidores para que o usuário não veja uma tela vazia.

2. Uso da Biblioteca RxJS
A biblioteca RxJS foi fundamental para a comunicação com o back-end através de Observables. Usei o método .subscribe() para que o front-end fique "escutando" as APIs de forma assíncrona. Isso permite que o sistema continue funcionando enquanto espera a resposta do servidor, atualizando a lista de produtos ou o status da nota apenas quando o dado chega com sucesso.

3. Outras Bibliotecas e Finalidades
No Angular, a principal biblioteca externa utilizada foi o HttpClientModule, necessária para realizar as requisições HTTP (GET, POST, PUT, DELETE) entre o navegador e as APIs em C#. No back-end, utilizei o Microsoft.EntityFrameworkCore como o ORM para facilitar a comunicação e a persistência dos dados no banco SQL.

4. Componentes Visuais
Optei por não utilizar bibliotecas de componentes prontas (como Bootstrap ou Angular Material). Toda a interface foi construída do zero utilizando HTML e CSS puros, fazendo uso de CSS Grid e Flexbox. O objetivo foi criar uma aplicação leve, rápida e demonstrar domínio sobre a estilização e o layout da página.

5. Frameworks Utilizados (C#)
O back-end foi desenvolvido utilizando o framework ASP.NET Core para a criação das Web APIs. Como ORM para gerenciar o banco de dados e as tabelas, utilizei o Entity Framework Core.

6. Tratamento de Erros e Exceções
O tratamento de falhas foi focado na resiliência do sistema. Utilize blocos try/catch especialmente na integração entre os microsserviços. Se a API de estoque estiver fora do ar ou o produto não tiver saldo, o sistema de faturamento captura essa exceção e devolve uma mensagem tratada para o usuário, garantindo que a nota fiscal continue com o status "Aberta" para que o trabalho não seja perdido.

7. Uso do LINQ e Expressões Lambda
O LINQ foi amplamente utilizado para simplificar a manipulação de dados e as consultas ao banco.

Consultas: Usei métodos como .ToListAsync() para listar produtos e .FirstOrDefaultAsync() para buscas por ID.

Lógica: Usei o .Max() para calcular automaticamente o próximo número sequencial da nota fiscal com base no que já existe no banco.

Lambdas: Usei expressões como x => x.Id nos arquivos de mapeamento (Maps) para indicar ao banco de dados quais campos são as chaves primárias e como os relacionamentos de um-para-muitos devem funcionar.
