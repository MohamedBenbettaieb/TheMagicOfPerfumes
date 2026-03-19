using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using TheMagicOfPerfumes.Models;
using TheMagicOfPerfumes.Services.Interfaces;
using TheMagicOfPerfumes.ViewModels.Base;

namespace TheMagicOfPerfumes.ViewModels
{
    public partial class CategoryViewModel : ViewModelBase
    {
        private readonly ICategoryService _categoryService;

        [ObservableProperty]
        private ObservableCollection<Category> _categories = new();

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(SaveCommand))]
        private string _newCategoryName = string.Empty;

        [ObservableProperty]
        private Category? _selectedCategory;

        [ObservableProperty]
        private bool _isEditing = false;

        [ObservableProperty]
        private string _errorMessage = string.Empty;

        [ObservableProperty]
        private bool _isDeleteConfirmationVisible = false;

        [ObservableProperty]
        private Category? _categoryToDelete;

        public CategoryViewModel(ICategoryService categoryService)
        {
            _categoryService = categoryService;
            _ = LoadCategoriesAsync();
        }

        private async Task LoadCategoriesAsync()
        {
            try
            {
                var categories = await _categoryService.GetAllAsync();
                Categories = new ObservableCollection<Category>(categories);
                ErrorMessage = string.Empty;
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error loading categories: {ex.Message}";
            }
        }

        [RelayCommand(CanExecute = nameof(CanSave))]
        private async Task SaveAsync()
        {
            try
            {
                var trimmedName = NewCategoryName.Trim();

                if (string.IsNullOrWhiteSpace(trimmedName))
                {
                    ErrorMessage = "Category name cannot be empty.";
                    return;
                }

                int? excludeId = IsEditing ? SelectedCategory?.Id : null;
                var exists = await _categoryService.ExistsWithNameAsync(trimmedName, excludeId);

                if (exists)
                {
                    ErrorMessage = "A category with this name already exists.";
                    return;
                }

                if (IsEditing && SelectedCategory != null)
                {
                    SelectedCategory.Name = trimmedName;
                    await _categoryService.UpdateAsync(SelectedCategory);
                }
                else
                {
                    var newCategory = new Category { Name = trimmedName };
                    await _categoryService.AddAsync(newCategory);
                }

                await LoadCategoriesAsync();
                CancelEdit();
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error saving category: {ex.Message}";
            }
        }

        private bool CanSave() => !string.IsNullOrWhiteSpace(NewCategoryName?.Trim());

        [RelayCommand]
        private void EditCategory(Category category)
        {
            SelectedCategory = category;
            NewCategoryName = category.Name;
            IsEditing = true;
            ErrorMessage = string.Empty;
        }

        [RelayCommand]
        private void ShowDeleteConfirmation(Category category)
        {
            CategoryToDelete = category;
            IsDeleteConfirmationVisible = true;
        }

        [RelayCommand]
        private async Task ConfirmDeleteAsync()
        {
            if (CategoryToDelete != null)
            {
                try
                {
                    await _categoryService.DeleteAsync(CategoryToDelete);
                    await LoadCategoriesAsync();
                    if (SelectedCategory?.Id == CategoryToDelete.Id)
                    {
                        CancelEdit();
                    }
                    CancelDelete();
                }
                catch (Exception ex)
                {
                    ErrorMessage = $"Error deleting category: {ex.Message}";
                }
            }
        }

        [RelayCommand]
        private void CancelDelete()
        {
            CategoryToDelete = null;
            IsDeleteConfirmationVisible = false;
        }

        [RelayCommand]
        private void CancelEdit()
        {
            SelectedCategory = null;
            NewCategoryName = string.Empty;
            IsEditing = false;
            ErrorMessage = string.Empty;
        }

        partial void OnNewCategoryNameChanged(string value)
        {
            ErrorMessage = string.Empty;
        }
    }
}