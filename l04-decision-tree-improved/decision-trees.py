import pandas as pd
import numpy as np
folder = "C:\\Users\\Mahmoud\\projects\\ml-lessons\\l04-decision-tree-improved\\"
data = pd.read_csv(folder + "heart.csv")
data.describe()

data.hist()

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

# building tree using scikti learn 
from sklearn import tree
dtc = tree.DecisionTreeClassifier()
model = dtc.fit(x_train,y_train)
y_predicted = model.predict(x_test)

# building model using light GBM
import lightgbm
params = {
    'learning_rate': 0.01 , 
    'application': 'binary',
    'objective': 'binary',
    'metric': 'auc',
    'boosting': 'gbdt',
    'num_leaves': 10,
    'verbose': 1
}
l_train = lightgbm.Dataset(x_train, y_train)
l_test = lightgbm.Dataset(x_test,y_test)
l_model = lightgbm.train(params,l_train,valid_sets=l_test,num_boost_round=2000)
print(l_model.model_to_string())

l_model.best_iteration