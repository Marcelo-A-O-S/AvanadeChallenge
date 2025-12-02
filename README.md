## Desafio Técnico Avanade

Sistema de e-commerce com arquitetura de microserviços para gerenciamento de estoque de produtos e vendas.  

Esta solução foi desenvolvida como parte de um desafio técnico da Avanade, e demonstra conhecimentos em backend, frontend, orquestração de serviços, autenticação, banco de dados e comunicação entre serviços.

## ✨ Funcionalidades principais

- Cadastro, atualização e listagem de produtos.  
- Gerenciamento de estoque (quantidade, disponibilidade).  
- Processamento de vendas/compras — checkout, controle de pedidos.  
- Autenticação e autorização (login, JWT, controle de acesso).  
- Comunicação entre microserviços (via filas/mensageria — por exemplo RabbitMQ / outra solução conforme configuração).  
- Interface web para frontend (consumindo APIs do backend).  

## 🛠️ Tecnologias utilizadas

- Backend: C#, .NET, microsserviços, API Gateway, Docker, Docker Compose.  
- Banco de dados: PostgreSQL.  
- Mensageria / comunicação entre serviços: RabbitMQ (ou similar — conforme configuração).  
- Frontend: Next.js, TailwindCSS, Shadcn-UI, Axios, etc.  
- Autenticação: JWT (JSON Web Tokens).  
- Infraestrutura / containerização: Docker, Docker Compose.  

## 📁 Estrutura do repositório

````
D:\AvanadeChallenge\
│
├───Project
│   ├───AuthService
│   ├───SaleService
│   ├───StockService
│   ├───Gateway
│
└───Frontend
    ├───Website/
    └───Mobile/ <- Caso haja a necessidade de expandir o projeto
````

## ✅ O que já está implementado / status atual

- [X] Microsserviços backend com CRUD de produtos e controle de estoque  
- [X] Banco de dados PostgreSQL + dockerização  
- [X] Frontend consumindo APIs e interface básica de e-commerce  
- [X] Autenticação via JWT    
- [X] Documentação de APIs (ex: swagger / open-api) — opcional mas recomendado  

## 🎯 Objetivo do Projeto

- Demonstrar domínio de **arquitetura de microsserviços**, comunicações entre serviços, containerização e integração entre backend + frontend.  
- Servir como portfólio técnico para desafios, entrevistas e demonstração de habilidades.  

## 💡 Como contribuir

Interessado em contribuir? Você pode:

- Reportar bugs abrindo uma *issue*  
- Sugerir ou implementar novas funcionalidades (ex: testes, documentação, melhorias de UI/UX)  
- Melhorar a documentação ou adicionar exemplos de uso  

Para contribuir:

1. Faça um *fork* do repositório.  
2. Crie uma *branch* com sua feature/fix: `git checkout -b minha-feature`.  
3. Suba as mudanças e abra um *Pull Request*.  

## 📄 Licença

Este projeto está sob a licença **MIT** — sinta-se livre para usar, modificar e distribuir, desde que mantenha os créditos originais.

---

Feito com 💙 por Marcelo — sinta-se à vontade para explorar, sugerir melhorias ou usar como base para seus projetos.
