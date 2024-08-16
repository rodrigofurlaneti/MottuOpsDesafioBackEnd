using MottuOpsDesafioBackEnd.Domain.Models;
using MottuOpsDesafioBackEnd.Business.Interface;
using MottuOpsDesafioBackEnd.Data.Interface;

namespace MottuOpsDesafioBackEnd.Business.Service
{
    public class ProfileService : IProfileService
    {
        private readonly IProfileRepository _profileRepository;

        public ProfileService(IProfileRepository profileRepository)
        {
            _profileRepository = profileRepository;
        }

        public async Task DeleteAsync(int userProfileId)
        {
            try
            {
                await _profileRepository.DeleteAsync(userProfileId);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Erro ao deletar o perfil de usuário com ID {userProfileId}", ex.Message);

                throw new ApplicationException($"Erro ao deletar o perfil de usuário com ID {userProfileId}", ex);
            }
        }

        public Task<IEnumerable<UserProfileModel>> GetAllAsync()
        {
            try
            {
                return _profileRepository.GetAllAsync();
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Erro ao listar todos os perfis de usuários", ex);
            }
        }

        public Task<UserProfileModel> GetByIdAsync(int userProfileId)
        {
            try
            {
                return _profileRepository.GetByIdAsync(userProfileId);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Erro ao selecionar o perfil de usuário com ID {userProfileId}", ex.Message);

                throw new ApplicationException($"Erro ao selecionar o usuário com ID {userProfileId}", ex);
            }
        }

        public Task<int> PostAsync(UserProfileModel userModel)
        {
            try
            {
                return _profileRepository.PostAsync(userModel);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Erro ao inserir um novo perfil de usuário com ID {userModel.ProfileName}", ex.Message);

                throw new ApplicationException($"Erro ao inserir um novo perfil de usuário com ID {userModel.ProfileName}", ex);
            }
        }

        public Task PutAsync(UserProfileModel userProfileModel)
        {
            try
            {
                return _profileRepository.PutAsync(userProfileModel);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Erro ao atualizar um perfil de usuário com ID {userProfileModel.ProfileName}", ex.Message);

                throw new ApplicationException($"Erro ao atualizar um perfil de usuário com ID {userProfileModel.ProfileName}", ex);
            }
        }
    }
}
