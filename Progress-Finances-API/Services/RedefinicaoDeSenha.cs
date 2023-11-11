
using Progress_Finances_API.Data;
using System.Security.Cryptography;

namespace Progress_Finances_API.Services
{
    public class RedefinicaoDeSenha
    {

        private readonly DataContext _dc;
        private static readonly Random random = new Random();
        public RedefinicaoDeSenha(DataContext dc)
        {
            _dc = dc;
        }


        //public string RedefinirSenha(string email, string senha)
        //{
        //    var email = _dc.f
        //    /*
        //     BACK
        //                    REDEFINIR SENHA - CANCELADO POR JÁ TER UM REDEFINIR SENHA CONTROLLER E TER A VERIFICACAODEEMAIL ANTES DE CHAMAR O REDEFINIRSENHA
        //        1- email, senha
        //        2- verificar se esse email existe
        //        3- trata se o email não existir
        //        4- se o email existir ->  chama o gerar token

        //                    GERAR CODIGO - ESTOU AQUI!
        //        5- gera o token temporario de no maximo 10 minutos
        //        6- salva no banco
        //        7- envia o token gerado para o email

        //                    VERIFICA CODIGO GERADO
        //        8- verifica se o token digitado esta correto
        //        9- se tiver correto, então, salva no banco a nova senha

        //    FRONT 
        //                    CHAMA O ENDPOINT REDEFINIR SENHA
        //        1- tela de redefinir senha
        //        2- coloca o email e a senha
        //        3- envia essas 2 informações para o back

        //                    TELA PARA ADICIONAR O TOKEN GERADO
        //        4- já chama a tela de adicionar o token

        //                    CHAMA O ENDPOINT GERAR CODIGO
        //        5- caso o emai não exista, o back vai lançar uma esessão (vc pega e joga na tela "E-mail não encontrado")
        //        6- se existe, então espera o usuario colocar o token

        //                    CHAMA O ENDPOINT VERIFICA CODIGO GERADO
        //        7- verifica se esse token é o mesmo que esta salvo no banco


        //      OU
        //          Antes de chamar o redefinir senha, vai chamar o VERIFICAR EMAIL
        //              Cria um endpoint separado apenas para verificar se o email existe
        //              se o email existir ele já vai chamar o RedefinirSenha

        //          SE DER SUCESSO CHAMAR O REDEFINIR SENHA
        //          SENÃO RETORNA A MSG DE E-MAIL NÃO ENCONTRADO


        //     */
        //    return null;
        //}

        public string GerarToken(string email)
        {
            /*
                             GERAR CODIGO
                5- gera o token temporario de no maximo 10 minutos
                6- salva no banco
                7- envia o token gerado para o email
             */
            //5 - gera o token

            int geraToken = random?.Next(1000, 10000) ?? 0;
            var token = geraToken.ToString().Substring(0, 4);

            //6 - salva no banco
            var db = _dc.usuarios.FirstOrDefault(u => u.Email == email);
            if (db != null)
            {
                db.Token = token;
                _dc.usuarios.Update(db);
                _dc.SaveChanges();
            }

            //7 - envia o token gerado para o email
            EnviarEmail(email, token);

            return token;
        }

        public string EnviarEmail(string email, string token)
        {

            //AO INVES DISSO TENTE INJETAR O EMAILSERVICE NO CONTRUTOR E CHAMAR O SENEMAIL PASSANDO OS PARAMETROS DESEJADOS
            var enviarTokenPorEmail = new EmailService("smtp.gmail.com", EnvioDeEmailProp.Email, EnvioDeEmailProp.Senha, token);
            enviarTokenPorEmail.SendEmail(email, "Redefinição de Senha", "template do token"); //adicionar o template do token

            return "Ok";
        }

        public string VerificarTokenGerado(string email, string token)
        {
            /*
             
                            VERIFICA CODIGO GERADO
                8- verifica se o token digitado esta correto
                9- se tiver correto, então, salva no banco a nova senha
             */

            var db = _dc.usuarios.FirstOrDefault(u => u.Email == email);
            if (db != null)
            {
                if (token == db.Token)
                {
                    db.Token = null;
                    _dc.usuarios.Update(db);
                    _dc.SaveChanges();

                    return "OK";
                }
            }

            return null;
        }

        public void VerificaEmail(string email)
        {
            var db = _dc.usuarios.FirstOrDefault(u => u.Email == email);
            if (db != null)
            {
                GerarToken(email);
            }
        }
    }
}
