using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Blockchain;
using Blockchain.Models;
using Ninject;
using Registrar.Database.Interfaces;

namespace Registrar.Api
{
    public static class MutliChainConfig
    {
        public static void StartBlockchains()
        {
            var handler = NinjectWebCommon.Kernel.Get<MultiChainHandler>();
            var blockchainStore = NinjectWebCommon.Kernel.Get<IRegiBlockchainStore>();
            var blockchainsTask = blockchainStore.GetAllBlockchains();
            blockchainsTask.Wait();

            Task.WaitAll(
                blockchainsTask.Result.Select(
                        blockchain =>
                            // We're the host/master node, so local port = remote port
                                handler.Connect(IPAddress.Loopback.ToString(), blockchain.ChainString, blockchain.Port,
                                    blockchain.Port, MultiChainTools.GetNewPort(EPortType.Rpc), false))
                    .Cast<Task>()
                    .ToArray());
        }

        public static void StopBlockchains()
        {
            var handler = NinjectWebCommon.Kernel.Get<MultiChainHandler>();

            Task.WaitAll(
                handler.Connections.Values.Select(connection => handler.DisconnectAndClose(connection)).ToArray());
        }
    }
}