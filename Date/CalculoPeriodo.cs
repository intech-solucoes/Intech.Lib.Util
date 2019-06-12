using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Intech.Lib.Util.Date
{
    public class CalculoPeriodo
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="periodos"></param>
        public Intervalo CalculaDuracao(List<Periodo> periodos)
        {
            return CalculaDuracao(periodos, new CalculoAnosMesesDiasAlgoritmo1());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="periodos"></param>
        public Intervalo CalculaDuracao(List<Periodo> periodos, CalculoAnosMesesDiasStrategy strategy)
        {
            return CalculaDuracao(periodos, null, null, strategy);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="periodos"></param>
        /// <param name="dataLimiteInicial">É um limitante inicial para o cálculo. Se for informado
        /// apenas as datas após esta data serão consideradas.</param>
        /// <param name="dataLimiteFinal">É um limitante final para o cálculo. Se for informado
        /// apenas as datas até esta data serão consideradas.</param>
        public Intervalo CalculaDuracao(List<Periodo> periodos, DateTime? dataLimiteInicial, DateTime? dataLimiteFinal)
        {
            return CalculaDuracao(periodos, dataLimiteInicial, dataLimiteFinal, new CalculoAnosMesesDiasAlgoritmo1());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="periodos"></param>
        /// <param name="dataLimiteInicial">É um limitante inicial para o cálculo. Se for informado
        /// apenas as datas após esta data serão consideradas.</param>
        /// <param name="dataLimiteFinal">É um limitante final para o cálculo. Se for informado
        /// apenas as datas até esta data serão consideradas.</param>
        public Intervalo CalculaDuracao(List<Periodo> periodos, DateTime? dataLimiteInicial, DateTime? dataLimiteFinal, CalculoAnosMesesDiasStrategy strategy)
        {
            Intervalo intervalo = new Intervalo(strategy);

            periodos = periodos.OrderBy(x => x.DataInicio).ToList();

            foreach (Periodo periodo in periodos)
            {
                DateTime dataInicio = periodo.DataInicio;
                DateTime dataTermino = periodo.DataTermino;

                if (dataLimiteFinal.HasValue)
                {
                    //se a data de início do período for após a Data do Limite Final para o cálculo
                    //este perído não será utilizado no cálculo e passamos para o próximo período
                    //imediatamente
                    if (periodo.DataInicio > dataLimiteFinal) continue;

                    //a data de término é a menor data entre a data de término do período e a data
                    //do limite final para o cálculo
                    dataTermino = DateTime.Compare(periodo.DataTermino, dataLimiteFinal.Value) > 0 ? dataLimiteFinal.Value : periodo.DataTermino;
                }

                if (dataLimiteInicial.HasValue)
                {
                    //se a data de término do período for  menor que a Data do Limite Inicial para o cálculo
                    //esse período não será utilizado no cálculo e passamos para o próximo período imediatamente
                    if (periodo.DataTermino < dataLimiteInicial) continue;

                    //a data inicial é sempre utilizada para obter período após uma determinada data
                    //o "APÓS" significa que a data corrente não é computada por isso é adicionado 1 dias
                    //à data inicial
                    dataInicio = DateTime.Compare(dataLimiteInicial.Value.AddDays(1), periodo.DataInicio) > 0 ? dataLimiteInicial.Value.AddDays(1) : periodo.DataInicio;
                }

                intervalo.Adiciona(new Intervalo(dataTermino, dataInicio, strategy));
            }

            return intervalo;
        }
    }
}
