namespace Algorithm.Logic
{
    using System;
    using System.Text.RegularExpressions;

    public class Program
    {
        /// <summary>
        /// PROBLEMA:
        /// 
        /// Implementar um algoritmo para o controle de posição de um drone emum plano cartesiano (X, Y).
        /// 
        /// O ponto inicial do drone é "(0, 0)" para cada execução do método Evaluate ao ser executado cada teste unitário.
        /// 
        /// A string de entrada pode conter os seguintes caracteres N, S, L, e O representando Norte, Sul, Leste e Oeste respectivamente.
        /// Estes catacteres podem estar presentes aleatóriamente na string de entrada.
        /// Uma string de entrada "NNNLLL" irá resultar em uma posição final "(3, 3)", assim como uma string "NLNLNL" irá resultar em "(3, 3)".
        /// 
        /// Caso o caracter X esteja presente, o mesmo irá cancelar a operação anterior. 
        /// Caso houver mais de um caracter X consecutivo, o mesmo cancelará mais de uma ação na quantidade em que o X estiver presente.
        /// Uma string de entrada "NNNXLLLXX" irá resultar em uma posição final "(1, 2)" pois a string poderia ser simplificada para "NNL".
        /// 
        /// Além disso, um número pode estar presente após o caracter da operação, representando o "passo" que a operação deve acumular.
        /// Este número deve estar compreendido entre 1 e 2147483647.
        /// Deve-se observar que a operação 'X' não suporta opção de "passo" e deve ser considerado inválido. Uma string de entrada "NNX2" deve ser considerada inválida.
        /// Uma string de entrada "N123LSX" irá resultar em uma posição final "(1, 123)" pois a string pode ser simplificada para "N123L"
        /// Uma string de entrada "NLS3X" irá resultar em uma posição final "(1, 1)" pois a string pode ser siplificada para "NL".
        /// 
        /// Caso a string de entrada seja inválida ou tenha algum outro problema, o resultado deve ser "(999, 999)".
        /// 
        /// OBSERVAÇÕES:
        /// Realizar uma implementação com padrões de código para ambiente de "produção". 
        /// Comentar o código explicando o que for relevânte para a solução do problema.
        /// Adicionar testes unitários para alcançar uma cobertura de testes relevânte.
        /// </summary>
        /// <param name="input">String no padrão "N1N2S3S4L5L6O7O8X"</param>
        /// <returns>String representando o ponto cartesiano após a execução dos comandos (X, Y)</returns>

        private const string invalidCoordinate = "(999, 999)";

        public static string Evaluate(string inputNew)
        {
            if (String.IsNullOrEmpty(inputNew) || inputNew.ToUpper().StartsWith("X"))
            {
                return invalidCoordinate;
            }

            var input = inputNew.ToUpper();
            if (Regex.Match(input, @"^[0-9]").Success
             || Regex.Match(input, @"[xX]+[0-9]").Success
             || Regex.Match(input, @"[^NLSOX/0-9]").Success)
            {
                return invalidCoordinate;
            }

            var cleanedInput = Regex.Replace(input, @"[NLSO]X", "");
            do
            {
                cleanedInput = Regex.Replace(cleanedInput, @"[NLSO]X", "");
            }
            while (Regex.Match(cleanedInput, @"[NLSO]X").Success);

            var cleanedInputNumberPlusX = Regex.Replace(cleanedInput, @"[NLSO][0-9]{1,10}X", "");
            if (Regex.Match(cleanedInputNumberPlusX, @"[NLSO][0-9]{11}").Success)
            {
                return invalidCoordinate;
            }

            var verifyStepValue = Regex.Matches(cleanedInputNumberPlusX, @"[0-9]{1,10}");

            foreach (var item in verifyStepValue)
            {
                var value = Int64.Parse(item.ToString());

                if (value > 2147483647)
                {
                    return invalidCoordinate;
                }
            }

            var result = CalcCoordinate(cleanedInputNumberPlusX);

            return result;
        }

        public static string CalcCoordinate(string input)
        {
            long _n = 0, _s = 0, _l = 0, _o = 0;

            var matchesToSum = Regex.Matches(input, @"[NLSO][0-9]{1,10}");

            foreach (var item in matchesToSum)
            {
                var coordinate = Regex.Match(item.ToString(), @"[NLSO]");
                var coordinateSteps = Regex.Match(item.ToString(), @"[0-9]{1,10}");

                switch (coordinate.Value)
                {
                    case "N":
                        _n = Int64.Parse(coordinateSteps.Value);
                        break;
                    case "S":
                        _s = Int64.Parse(coordinateSteps.Value);
                        break;
                    case "L":
                        _l = Int64.Parse(coordinateSteps.Value);
                        break;
                    case "O":
                        _o = Int64.Parse(coordinateSteps.Value);
                        break;
                    default:
                        break;
                }
            }

            var remainingCoordinates = Regex.Replace(input, @"[NLSO][0-9]{1,10}", "");

            var listChars = remainingCoordinates.ToCharArray();

            for (int i = 0; i < listChars.Length; i++)
            {
                switch (listChars[i])
                {
                    case 'N':
                        _n++;
                        break;
                    case 'S':
                        _s++;
                        break;
                    case 'L':
                        _l++;
                        break;
                    case 'O':
                        _o++;
                        break;
                    default:
                        break;
                }
            }

            var _x = _l - _o;
            var _y = _n - _s;

            if ((_x > 2147483647 || _y > 2147483647) || (_x < -2147483647 || _y < -2147483647))
            {
                return invalidCoordinate;
            }

            return $"({_x}, {_y})";
        }
    }
}
