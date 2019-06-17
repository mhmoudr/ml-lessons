
import os
import pandas as pd
import numpy as np
import matplotlib.pyplot as plt


#folder = "C:\\Users\\Mahmoud\\projects\\ml-lessons\\l04-decision-tree-improved\\"
folder  = "/Users/mahmoudr/playground/ml-lessons/l04-decision-tree-improved/"
data = pd.read_csv(folder + "heart.csv")
data.describe()

data.hist(figsize=(12,12))

data.columns

features = ['age', 'sex', 'cp', 'trestbps', 'chol', 'fbs', 'restecg', 'thalach', 'exang', 'oldpeak', 'slope', 'ca', 'thal']

x = data[features]
y = data.target

msk = np.random.rand(len(data)) < 0.75
train = data[msk]
test = data[~msk]
x_train = train[features].values
x_test = test[features].values
y_train = train.target.values
y_test = test.target.values

# # other datasets
# import sklearn.datasets as ds
# d = ds.load_boston()
# df = pd.DataFrame(d.data)
# df.columns = d.feature_names
# df.describe()


# building tree using scikti learn 
from sklearn import tree
dtc = tree.DecisionTreeClassifier()
model = dtc.fit(x_train,y_train)
y_predicted = model.predict(x_test)

# building model using light GBM
import lightgbm

l_train = lightgbm.Dataset(x_train, y_train)
l_test = lightgbm.Dataset(x_test,y_test)

l_params = {
    'learning_rate': 0.01 , 
    'application': 'binary',
    'objective': 'binary',
    'metric': 'binary_logloss',
    'boosting': 'gbdt',
    'num_leaves': 10,
    'verbose': 0
}
l_progress = dict()
l_model = lightgbm.train(l_params,
                         l_train,
                         valid_sets=[l_train, l_test],
                         num_boost_round=2000, 
                         evals_result=l_progress,
                         verbose_eval=10,
                         feature_name=features)
plt.rcParams['figure.figsize'] = [10, 7]
lightgbm.plot_metric(l_progress)
lightgbm.plot_tree(l_model,tree_index=1,figsize=(60,60),show_info=['split_gain'])
lightgbm.plot_importance(l_model)
# building trees using XG_Boost
import xgboost 

g_train = xgboost.DMatrix(x_train,y_train)
g_test = xgboost.DMatrix(x_test,y_test) 

g_params = {
    "objective":"binary:logistic",
    'colsample_bytree': 0.3,
    'learning_rate': 0.05,
    #'tree_method': 'hist',
    'max_depth': 6,
    'eval_metric' : 'logloss'
}
iterations = 200
progress = dict()
%time g_model = xgboost.train(g_params, g_train, evals=[(g_train,'training'), (g_test,'test')], num_boost_round=iterations, evals_result=progress) 


plt.rcParams['figure.figsize'] = [10, 7]
plt.plot(range(0,iterations),progress['training']['logloss'],progress['test']['logloss'])
plt.rcParams['figure.figsize'] = [30, 30]
xgboost.plot_tree(g_model,num_trees=0)
plt.rcParams['figure.figsize'] = [10, 10]
xgboost.plot_importance(g_model)
